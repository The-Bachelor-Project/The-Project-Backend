using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json.Linq;

namespace Data.Fetcher.YahooFinanceFetcher;

public class StockFetcher : IStockFetcher
{
	public async Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval)
	{
		System.Console.WriteLine("Fetching stock history for " + ticker + " on " + exchange + " from " + startDate + " to " + endDate);

		SqlConnection connection = new Database.Connection().Create();
		String getCurrencyQuery = "SELECT currency FROM Exchanges WHERE symbol = @symbol";
		SqlCommand command = new SqlCommand(getCurrencyQuery, connection);
		command.Parameters.AddWithValue("@symbol", exchange);
		SqlDataReader reader = command.ExecuteReader();

		if (!reader.Read())
		{
			throw new Exception("Exchange not found");
		}

		String currency = reader["currency"].ToString()!;

		int startTime = Tools.TimeConverter.dateOnlyToUnix(startDate);
		int endTime = Tools.TimeConverter.dateOnlyToUnix(endDate);
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
		String currencySymbol = Data.Database.Exchange.GetCurrency(exchange);

		StockHistory result = new StockHistory(ticker, exchange, startDate, endDate, "daily");
		List<string> dataList = dataLines.ToList();
		dataList.RemoveAt(0);

		foreach (string data in dataList)
		{
			String[] dataSplit = data.Split(",");
			DateOnly date = DateOnly.Parse(dataSplit[0]);
			if (date >= startDate && date <= endDate && dataSplit[1] != "null")
			{
				Data.DatePrice dataPoint = new Data.DatePrice(
					DateOnly.Parse(dataSplit[0]),
					new Data.Money(Decimal.Parse(dataSplit[1]), currency),
					new Data.Money(Decimal.Parse(dataSplit[2]), currency),
					new Data.Money(Decimal.Parse(dataSplit[3]), currency),
					new Data.Money(Decimal.Parse(dataSplit[4]), currency)
				);
				result.history.Add(dataPoint);
			}
		}

		await new Tools.PriceHistoryConverter().Convert(result.history, "USD");

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


		HttpResponseMessage quoteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/quote?symbols=" + tickerExt);
		String quoteJson = await quoteRes.Content.ReadAsStringAsync();
		dynamic quote = JObject.Parse(quoteJson);
		System.Console.WriteLine(quoteJson);

		result.ticker = ticker;
		result.exchange = exchange;
		result.name = quote.quoteResponse.result[0].shortName;
		try
		{
			result.industry = quoteSummary.quoteSummary.result[0].assetProfile.industry;
			result.sector = quoteSummary.quoteSummary.result[0].assetProfile.sector;
			result.website = quoteSummary.quoteSummary.result[0].assetProfile.website;
			result.country = quoteSummary.quoteSummary.result[0].assetProfile.country;
		}
		catch (Exception)
		{
			result.industry = "";
			result.sector = "";
			result.website = "";
			result.country = "";
		}


		if (result.name == null) //FIXME this is a botch solution
		{
			result.name = quote.quoteResponse.result[0].longName;
		}
		if (result.website == null) //FIXME this is a botch solution
		{
			result.website = "";
		}

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
					resultStocks = resultStocks.Append(await (new StockFetcher()).GetProfile(ticker, exchange)).ToArray();
				}
				else
				{
					using (SqlConnection connection = new Data.Database.Connection().Create()) //TODO: This is just for development. Remove before production.
					{
						String sqlQuery = "INSERT INTO MissingExchanges (exchange, disp, stock) VALUES (@exchange, @disp, @stock)";
						SqlCommand command = new SqlCommand(sqlQuery, connection);
						command.Parameters.AddWithValue("@exchange", "" + res.exch);
						command.Parameters.AddWithValue("@disp", "" + res.exchDisp);
						command.Parameters.AddWithValue("@stock", "" + res.symbol);
						command.ExecuteNonQuery();
					}
				}
			}
		}
		return resultStocks;
	}
}