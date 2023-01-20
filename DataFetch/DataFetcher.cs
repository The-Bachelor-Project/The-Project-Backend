
using Newtonsoft.Json.Linq;

class DataFetcher{
	public static async Task<StockInfo> stock(String ticker, String exchange){
		StockInfo result = new StockInfo();
		HttpClient client = new HttpClient();

		HttpResponseMessage quoteSummaryRes = await client.GetAsync("https://query1.finance.yahoo.com/v11/finance/quoteSummary/" + ticker + "?modules=assetProfile");
		String quoteSummaryJson = await quoteSummaryRes.Content.ReadAsStringAsync();
		dynamic quoteSummary = JObject.Parse(quoteSummaryJson);


		HttpResponseMessage quoteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/quote?symbols=" + ticker);
		String quoteJson = await quoteRes.Content.ReadAsStringAsync();
		dynamic quote = JObject.Parse(quoteJson);

		String exchangeTranslated = quote.quoteResponse.result[0].exchange;

		System.Console.WriteLine(quote.quoteResponse.result[0].exchange);
		System.Console.WriteLine("test22");

		String yfExchange = quote.quoteResponse.result[0].exchange;

		switch(""+quote.quoteResponse.result[0].exchange){
			case "NMS": exchangeTranslated = "NASDAQ"; break;
			case "NYQ": exchangeTranslated = "NYSE"; break;
		}

		result.ticker = ticker;
		result.exchange = exchangeTranslated;
		result.name = quote.quoteResponse.result[0].shortName;
        result.industry = quoteSummary.quoteSummary.result[0].assetProfile.industry;
        result.sector = quoteSummary.quoteSummary.result[0].assetProfile.sector;
        result.website = quoteSummary.quoteSummary.result[0].assetProfile.website;
        result.country = quoteSummary.quoteSummary.result[0].assetProfile.country;

		return result;
	}
}