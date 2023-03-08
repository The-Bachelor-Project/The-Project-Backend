
using System.Data.SqlClient;
using CsvHelper;
using Newtonsoft.Json.Linq;

namespace BackendService;

class DataFetcher
{
	public static async Task<Data.StockProfile> stock(String ticker, String exchange)
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


	public static async Task<String[]> stockHistory(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
	//TODO this is not done at all
	{
		int startTime = TimeConverter.dateOnlyToUnix(startDate);
		int endTime = TimeConverter.dateOnlyToUnix(endDate);

		String tickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		StockProfile result = new StockProfile();
		HttpClient client = new HttpClient();

		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + tickerExt + "?interval=1d&period1=" + startTime + "&period2=" + endTime;

		HttpResponseMessage stockHistoryRes = await client.GetAsync(url);
		String stockHistoryCsv = await stockHistoryRes.Content.ReadAsStringAsync();

		String[] dataLines = stockHistoryCsv.Replace("\r", "").Split("\n");

		return dataLines;
	}

	public static async Task<String[]> CurrencyHistory(String currency, DateOnly startDate, DateOnly endDate)
	{
		String CorrectCurrency = currency + "USD=X";
		int StartTime = TimeConverter.dateOnlyToUnix(startDate);
		int EndTime = TimeConverter.dateOnlyToUnix(endDate);

		HttpClient client = new HttpClient();

		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + CorrectCurrency + "?interval=1d&period1=" + StartTime + "&period2=" + EndTime;

		HttpResponseMessage stockHistoryRes = await client.GetAsync(url);
		String stockHistoryCsv = await stockHistoryRes.Content.ReadAsStringAsync();

		String[] dataLines = stockHistoryCsv.Replace("\r", "").Split("\n");

		return dataLines;
	}


}