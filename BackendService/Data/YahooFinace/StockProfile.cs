using Data.Interfaces;
using Newtonsoft.Json.Linq;

namespace Data.YahooFinance;

class StockProfile : IStockProfile
{
	public async Task<Data.StockProfile> Get(string ticker, string exchange)
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

		throw new NotImplementedException();
	}
}