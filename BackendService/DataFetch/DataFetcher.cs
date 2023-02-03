
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

	public static async void StockHistory(String ticker, String exchange, int start_time, int end_time) //TODO: this is not done at all
	{
		String tickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		StockInfo result = new StockInfo();
		HttpClient client = new HttpClient();

		HttpResponseMessage stockHistoryRes = await client.GetAsync("https://query1.finance.yahoo.com/v7/finance/download/" + tickerExt + "?interval=1d&period1=" + start_time + "&period2=" + end_time);
		String stockHistoryJson = await stockHistoryRes.Content.ReadAsStringAsync();
		dynamic stockHistory = JObject.Parse(stockHistoryJson);
	}
}