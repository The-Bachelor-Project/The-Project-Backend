class DataFetcher{
	public static async Task<StockInfo> stock(String ticker, String exchange){
		StockInfo result = new StockInfo();
		HttpClient client = new HttpClient();

		HttpResponseMessage quoteSummaryRes = await client.GetAsync("https://query1.finance.yahoo.com/v11/finance/quoteSummary/" + ticker + "?modules=assetProfile");
		String quoteSummaryJson = await quoteSummaryRes.Content.ReadAsStringAsync();
		QuoteSummaryRoot quoteSummaryRoot =  System.Text.Json.JsonSerializer.Deserialize<QuoteSummaryRoot>(quoteSummaryJson);

		HttpResponseMessage quoteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/quote?symbols=" + ticker);
		String quoteJson = await quoteRes.Content.ReadAsStringAsync();
		QuoteRoot quoteRoot =  System.Text.Json.JsonSerializer.Deserialize<QuoteRoot>(quoteJson);

		result.ticker = ticker;
		result.exchange = exchange;
		result.name = quoteRoot.quoteResponse.result[0].shortName;
        result.industry = quoteSummaryRoot.quoteSummary.result[0].assetProfile.industry;
        result.sector = quoteSummaryRoot.quoteSummary.result[0].assetProfile.sector;
        result.website = quoteSummaryRoot.quoteSummary.result[0].assetProfile.website;
        result.country = quoteSummaryRoot.quoteSummary.result[0].assetProfile.country;

		return result;
	}
}