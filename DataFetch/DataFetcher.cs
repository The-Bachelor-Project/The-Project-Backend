
using Newtonsoft.Json.Linq;

class DataFetcher{
	public static async Task<StockInfo> stock(String ticker, String exchange){
		String stockExtension;
		YfTranslator.stockSymbolExtension.TryGetValue(exchange, out stockExtension);
		String tickerExt = ticker + stockExtension;

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
}