using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json.Linq;

namespace Data.Fetcher.YahooFinanceFetcher;

public class StockFetcher : IStockFetcher
{
	public async Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval)
	{
		System.Console.WriteLine("Fetching stock history for " + ticker + " on " + exchange + " from " + startDate + " to " + endDate);
		int StartTime = Tools.TimeConverter.dateOnlyToUnix(startDate);
		int EndTime = Tools.TimeConverter.dateOnlyToUnix(endDate);
		String TickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		HttpClient Client = new HttpClient();
		String Url = "https://query1.finance.yahoo.com/v7/finance/download/" + TickerExt + "?interval=1d&period1=" + StartTime + "&period2=" + EndTime;
		//System.Console.WriteLine(Url);
		HttpResponseMessage StockHistoryRes = await Client.GetAsync(Url);

		if (StockHistoryRes.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return new StockHistory(ticker, exchange, "daily");
		}

		String StockHistoryCsv = await StockHistoryRes.Content.ReadAsStringAsync();
		String[] DataLines = StockHistoryCsv.Replace("\r", "").Split("\n");
		String CurrencySymbol = Data.Database.Exchange.GetCurrency(exchange);

		//TODO update so the currency converter just returns ones when usd to usd, and then drop DoCurrencyConvert bool

		StockHistory Result = new StockHistory(ticker, exchange, startDate, endDate, "daily");
		List<string> DataList = DataLines.ToList();
		DataList.RemoveAt(0);

		foreach (string Data in DataList)
		{
			String[] DataSplit = Data.Split(",");
			DateOnly Date = DateOnly.Parse(DataSplit[0]);
			if (Date >= startDate && Date <= endDate)
			{
				Data.DatePrice DataPoint = new Data.DatePrice(
					DateOnly.Parse(DataSplit[0]),
					new StockApp.Money(Decimal.Parse(DataSplit[1])),
					new StockApp.Money(Decimal.Parse(DataSplit[2])),
					new StockApp.Money(Decimal.Parse(DataSplit[3])),
					new StockApp.Money(Decimal.Parse(DataSplit[4]))
				);
				Result.History.Add(DataPoint);
			}
		}

		return Result;
	}

	public async Task<Data.StockProfile> GetProfile(string ticker, string exchange)
	{
		String tickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		Data.StockProfile result = new Data.StockProfile();
		HttpClient client = new HttpClient();

		HttpResponseMessage quoteSummaryRes = await client.GetAsync("https://query1.finance.yahoo.com/v11/finance/quoteSummary/" + tickerExt + "?modules=assetProfile");
		String quoteSummaryJson = await quoteSummaryRes.Content.ReadAsStringAsync();
		dynamic quoteSummary = JObject.Parse(quoteSummaryJson);


		HttpResponseMessage quoteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/quote?symbols=" + tickerExt);
		String quoteJson = await quoteRes.Content.ReadAsStringAsync();
		dynamic quote = JObject.Parse(quoteJson);
		System.Console.WriteLine(quoteJson);

		result.Ticker = ticker;
		result.Exchange = exchange;
		result.Name = quote.quoteResponse.result[0].shortName;
		try
		{
			result.Industry = quoteSummary.quoteSummary.result[0].assetProfile.industry;
			result.Sector = quoteSummary.quoteSummary.result[0].assetProfile.sector;
			result.Website = quoteSummary.quoteSummary.result[0].assetProfile.website;
			result.Country = quoteSummary.quoteSummary.result[0].assetProfile.country;
		}
		catch (Exception)
		{
			result.Industry = "";
			result.Sector = "";
			result.Website = "";
			result.Country = "";
		}


		if (result.Name == null) //FIXME this is a botch solution
		{
			result.Name = quote.quoteResponse.result[0].longName;
		}
		if (result.Website == null) //FIXME this is a botch solution
		{
			result.Website = "";
		}

		return result;
	}

	public async Task<Data.StockProfile[]> Search(string query)
	{
		HttpClient Client = new HttpClient();
		HttpResponseMessage AutoCompleteRes = await Client.GetAsync("https://query1.finance.yahoo.com/v6/finance/autocomplete?region=US&lang=en&query=" + query);
		String AutoCompleteResJson = await AutoCompleteRes.Content.ReadAsStringAsync();
		dynamic AutoComplete = JObject.Parse(AutoCompleteResJson);

		JArray Results = AutoComplete.ResultSet.Result;

		Data.StockProfile[] ResultStocks = new Data.StockProfile[] { };

		foreach (dynamic res in Results)
		{
			if (res.type == "S" || res.type == "s")
			{
				string exchange = "";
				if (YfTranslator.stockAutocomplete.TryGetValue("" + res.exch, out exchange))
				{
					String ticker = ("" + res.symbol).Split(".")[0];
					ResultStocks = ResultStocks.Append(await (new StockFetcher()).GetProfile(ticker, exchange)).ToArray();
				}
				else
				{
					using (SqlConnection connection = new Data.Database.Connection().Create()) //TODO: This is just for development. Remove before production.
					{
						String SqlQuery = "INSERT INTO MissingExchanges (exchange, disp, stock) VALUES (@exchange, @disp, @stock)";
						SqlCommand command = new SqlCommand(SqlQuery, connection);
						command.Parameters.AddWithValue("@exchange", "" + res.exch);
						command.Parameters.AddWithValue("@disp", "" + res.exchDisp);
						command.Parameters.AddWithValue("@stock", "" + res.symbol);
						command.ExecuteNonQuery();
					}
				}
			}
		}
		return ResultStocks;
	}
}