using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json.Linq;

namespace Data.Fetcher.YahooFinanceFetcher;

public class StockFetcher : IStockFetcher
{
	public async Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval)
	{
		System.Console.WriteLine("Fetching stock history for " + ticker + " on " + exchange + " from " + startDate + " to " + endDate);

		String getCurrencyQuery = "SELECT currency FROM Exchanges WHERE symbol = @symbol";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@symbol", exchange);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getCurrencyQuery, parameters);


		if (data == null)
		{
			throw new StatusCodeException(400, "Exchange not found");
		}
		String currency = data["currency"].ToString()!;

		int startTime = Tools.TimeConverter.DateOnlyToUnix(startDate);
		int endTime = Tools.TimeConverter.DateOnlyToUnix(endDate);
		String tickerExt = YfTranslator.GetYfSymbol(ticker, exchange);

		HttpClient client = new HttpClient();
		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + tickerExt + "?interval=1d&period1=" + startTime + "&period2=" + endTime;
		System.Console.WriteLine(url);
		HttpResponseMessage stockHistoryRes = await client.GetAsync(url);

		if (stockHistoryRes.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return new StockHistory(ticker, exchange, "daily");
		}

		String stockHistoryCsv = await stockHistoryRes.Content.ReadAsStringAsync();
		String[] dataLines = stockHistoryCsv.Replace("\r", "").Split("\n");
		if (dataLines.Count() == 1)
		{
			return new StockHistory(ticker, exchange, "daily");
		}
		String currencySymbol = Data.Database.Exchange.GetCurrency(exchange);
		StockHistory result = new StockHistory(ticker, exchange, startDate, endDate, "daily");
		List<string> dataList = dataLines.ToList();
		dataList.RemoveAt(0);

		foreach (string row in dataList)
		{
			String[] dataSplit = row.Split(",");
			DateOnly date = DateOnly.Parse(dataSplit[0]);
			if (date >= startDate && date <= endDate && dataSplit[1] != "null")
			{
				try
				{
					Data.DatePriceOHLC dataPoint = new Data.DatePriceOHLC(
					DateOnly.Parse(dataSplit[0]),
					new Data.Money(Decimal.Parse(dataSplit[1]), currency),
					new Data.Money(Decimal.Parse(dataSplit[2]), currency),
					new Data.Money(Decimal.Parse(dataSplit[3]), currency),
					new Data.Money(Decimal.Parse(dataSplit[4]), currency)
					);
					result.history.Add(dataPoint);
				}
				catch (Exception e)
				{
					System.Console.WriteLine(e);
					continue;
				}

			}
		}
		await new Tools.PriceHistoryConverter().ConvertStockPrice(result.history, "USD");
		foreach (Data.DatePriceOHLC datePrice in result.history)
		{
			try
			{
				if (datePrice.highPrice.currency != "USD")
				{
					if (result.history.Count == 1)
					{
						result.history.Clear();
						break;
					}

					int indexOfWrongPrice = result.history.IndexOf(datePrice);
					if (indexOfWrongPrice - 1 > 0)
					{
						datePrice.highPrice.amount = result.history[indexOfWrongPrice - 1].highPrice.amount;
						datePrice.lowPrice.amount = result.history[indexOfWrongPrice - 1].lowPrice.amount;
						datePrice.openPrice.amount = result.history[indexOfWrongPrice - 1].openPrice.amount;
						datePrice.closePrice.amount = result.history[indexOfWrongPrice - 1].closePrice.amount;
					}
					else if (indexOfWrongPrice + 1 < result.history.Count - 1)
					{
						datePrice.highPrice.amount = result.history[indexOfWrongPrice + 1].highPrice.amount;
						datePrice.lowPrice.amount = result.history[indexOfWrongPrice + 1].lowPrice.amount;
						datePrice.openPrice.amount = result.history[indexOfWrongPrice + 1].openPrice.amount;
						datePrice.closePrice.amount = result.history[indexOfWrongPrice + 1].closePrice.amount;
					}
					datePrice.closePrice.currency = "USD";
					datePrice.highPrice.currency = "USD";
					datePrice.lowPrice.currency = "USD";
					datePrice.openPrice.currency = "USD";
				}
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				continue;
			}
		}
		return result;
	}

	public async Task<Data.StockProfile> GetProfile(string ticker, string exchange)
	{
		String tickerExt = YfTranslator.GetYfSymbol(ticker, exchange);

		Data.StockProfile result = new Data.StockProfile();
		HttpClient client = new HttpClient();

		HttpResponseMessage quoteSummaryRes = await client.GetAsync("https://query1.finance.yahoo.com/v11/finance/quoteSummary/" + tickerExt + "?modules=assetProfile");
		String quoteSummaryJson = await quoteSummaryRes.Content.ReadAsStringAsync();
		dynamic quoteSummary = JObject.Parse(quoteSummaryJson);
		if (quoteSummary.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			throw new StatusCodeException(404, "Could not get stock profile for " + exchange + ":" + ticker + ", using quoteSummary on Yahoo Finance");
		}


		HttpResponseMessage quoteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/quote?symbols=" + tickerExt);
		String quoteJson = await quoteRes.Content.ReadAsStringAsync();
		dynamic quote = JObject.Parse(quoteJson);
		System.Console.WriteLine(quote);
		if (quoteSummary.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			throw new StatusCodeException(404, "Could not get stock profile for " + exchange + ":" + ticker + ", using quote on Yahoo Finance");
		}


		result.ticker = ticker;
		result.exchange = exchange;
		try
		{
			result.displayName = quote.quoteResponse.result[0].displayName ??
			quote.quoteResponse.result[0].shortName ??
			quote.quoteResponse.result[0].longName ??
			throw new StatusCodeException(500, "Stock does not have a name");
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Stock " + exchange + ":" + ticker + " could not be gotten from Yahoo Finance. Please check if ticker and exchange are correct.");
		}
		result.shortName = quote.quoteResponse.result[0].shortName ?? "";
		result.longName = quote.quoteResponse.result[0].longName ?? "";
		result.sharesOutstanding = quote?.quoteResponse?.result?[0]?.sharesOutstanding ?? 0;
		result.financialCurrency = quote?.quoteResponse?.result?[0]?.financialCurrency ?? "";
		result.industry = quoteSummary!.quoteSummary!.result?[0]!.assetProfile!.industry ?? "";
		result.sector = quoteSummary?.quoteSummary?.result?[0]?.assetProfile?.sector ?? "";
		result.website = quoteSummary?.quoteSummary?.result?[0]?.assetProfile?.website ?? "";
		result.country = quoteSummary?.quoteSummary?.result?[0]?.assetProfile?.country ?? "";
		result.state = quoteSummary?.quoteSummary?.result?[0]?.assetProfile?.state ?? "";
		result.city = quoteSummary?.quoteSummary?.result?[0]?.assetProfile?.city ?? "";
		result.address = quoteSummary?.quoteSummary?.result?[0]?.assetProfile?.address1 ?? "";
		result.zip = quoteSummary?.quoteSummary?.result?[0]?.assetProfile?.zip ?? "";
		result.trailingAnnualDividendRate = quote?.quoteResponse?.result[0]?.trailingAnnualDividendRate ?? 0;
		result.trailingAnnualDividendYield = quote?.quoteResponse?.result[0]?.trailingAnnualDividendYield ?? 0;

		return result;
	}

	public async Task<Data.StockProfile[]> Search(string query)
	{
		HttpClient client = new HttpClient();
		HttpResponseMessage autoCompleteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/autocomplete?region=US&lang=en&query=" + query);
		String autoCompleteResJson = await autoCompleteRes.Content.ReadAsStringAsync();
		dynamic autoComplete = JObject.Parse(autoCompleteResJson);

		JArray results = autoComplete.ResultSet.Result;

		Data.StockProfile[] resultStocks = new Data.StockProfile[] { };

		foreach (dynamic res in results)
		{
			if (res.type == "S" || res.type == "s")
			{
				string exchange = "";
				if (YfTranslator.stockAutocomplete.TryGetValue("" + res.exch, out exchange))
				{
					String ticker = ("" + res.symbol).Split(".")[0];
					try
					{
						resultStocks = resultStocks.Append(await (new Data.Fetcher.StockFetcher()).GetProfile(ticker, exchange)).ToArray();
					}
					catch (Exception e)
					{
						//TODO maybe do something about this, i dunno
					}
				}
				else
				{
					SqlConnection connection = Data.Database.Connection.GetSqlConnection();
					String sqlQuery = "INSERT INTO MissingExchanges (exchange, disp, stock) VALUES (@exchange, @disp, @stock)";
					SqlCommand command = new SqlCommand(sqlQuery, connection);
					command.Parameters.AddWithValue("@exchange", "" + res.exch);
					command.Parameters.AddWithValue("@disp", "" + res.exchDisp);
					command.Parameters.AddWithValue("@stock", "" + res.symbol);
					command.ExecuteNonQuery();

				}
			}
		}
		return resultStocks;
	}

	public async Task<List<Dividend>> GetDividends(string ticker, string exchange, DateOnly startDate, DateOnly endDate)
	{
		System.Console.WriteLine("Getting dividends for " + ticker + " " + exchange);
		List<Data.Dividend> dividends = new List<Data.Dividend>();

		String getCurrencyQuery = "SELECT currency FROM Exchanges WHERE symbol = @symbol";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@symbol", exchange);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getCurrencyQuery, parameters);


		if (data == null)
		{
			throw new StatusCodeException(400, "Exchange of " + exchange + ":" + ticker + " was not found");
		}
		String currency = data["currency"].ToString()!;

		int startTime = Tools.TimeConverter.DateOnlyToUnix(startDate);
		int endTime = Tools.TimeConverter.DateOnlyToUnix(endDate);
		String tickerExt = YfTranslator.GetYfSymbol(ticker, exchange);

		HttpClient client = new HttpClient();
		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + tickerExt + "?interval=1d&period1=" + startTime + "&period2=" + endTime + "&events=div";
		System.Console.WriteLine(url);
		HttpResponseMessage dividendsResponse = client.GetAsync(url).Result;
		if (dividendsResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			throw new StatusCodeException(404, "The stock " + exchange + ":" + ticker + " was not found on Yahoo Finance");
		}
		String stockDividendCSV = await dividendsResponse.Content.ReadAsStringAsync();
		String[] stockDividendLines = stockDividendCSV.Split("\n");
		if (stockDividendLines.Length == 1)
		{
			return dividends;
		}
		String currencySymbol = Data.Database.Exchange.GetCurrency(exchange);
		List<String> dataList = stockDividendLines.ToList();
		dataList.RemoveAt(0);
		foreach (String dataP in dataList)
		{
			try
			{
				String[] dataSplit = dataP.Split(",");
				DateOnly date = DateOnly.Parse(dataSplit[0]);
				if (date >= startDate && date <= endDate && dataSplit[1] != "null")
				{
					Data.Dividend dataPoint = new Data.Dividend(
						DateOnly.Parse(dataSplit[0]),
						new Data.Money(Decimal.Parse(dataSplit[1]), currency)
					);
					dividends.Add(dataPoint);
				}
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				continue;
			}

		}
		await new Tools.PriceHistoryConverter().ConvertStockDividends(dividends, "USD");
		return dividends;
	}
}