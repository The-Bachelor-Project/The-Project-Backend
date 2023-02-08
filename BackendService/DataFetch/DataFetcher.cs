
using System.Data.SqlClient;
using CsvHelper;
using Newtonsoft.Json.Linq;

namespace BackendService;

class DataFetcher
{
	public static async Task<StockInfo> stock(String ticker, String exchange)
	{
		String tickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		StockInfo result = new StockInfo();
		HttpClient client = new HttpClient();

		HttpResponseMessage quoteSummaryRes = await client.GetAsync("https://query1.finance.yahoo.com/v11/finance/quoteSummary/" + tickerExt + "?modules=assetProfile");
		String quoteSummaryJson = await quoteSummaryRes.Content.ReadAsStringAsync();
		dynamic quoteSummary = JObject.Parse(quoteSummaryJson);


		HttpResponseMessage quoteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/quote?symbols=" + tickerExt);
		String quoteJson = await quoteRes.Content.ReadAsStringAsync();
		dynamic quote = JObject.Parse(quoteJson);

		result.ticker = ticker;
		result.exchange = exchange;
		result.name = quote.quoteResponse.result[0].shortName;
		result.industry = quoteSummary.quoteSummary.result[0].assetProfile.industry;
		result.sector = quoteSummary.quoteSummary.result[0].assetProfile.sector;
		result.website = quoteSummary.quoteSummary.result[0].assetProfile.website;
		result.country = quoteSummary.quoteSummary.result[0].assetProfile.country;

		return result;
	}


	public static async Task<String[]> stockHistory(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
	//TODO this is not done at all
	{
		int startTime = TimeConverter.dateOnlyToUnix(startDate);
		int endTime = TimeConverter.dateOnlyToUnix(endDate);

		String tickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		StockInfo result = new StockInfo();
		HttpClient client = new HttpClient();

		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + tickerExt + "?interval=1d&period1=" + startTime + "&period2=" + endTime;
		System.Console.WriteLine("URL: " + url);
		System.Console.WriteLine("URL: " + url);

		HttpResponseMessage stockHistoryRes = await client.GetAsync(url);
		String stockHistoryCsv = await stockHistoryRes.Content.ReadAsStringAsync();

		System.Console.WriteLine(stockHistoryCsv);
		String[] dataLines = stockHistoryCsv.Replace("\r", "").Split("\n");

		return dataLines;
	}
}