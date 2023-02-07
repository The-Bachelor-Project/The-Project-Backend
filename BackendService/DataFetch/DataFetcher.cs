
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

	public static async void StockHistory(String ticker, String exchange, DateOnly startDate, DateOnly endDate) //TODO: this is not done at all
	{
		int startTime = TimeConverter.dateOnlyToUnix(startDate);
		int endTime = TimeConverter.dateOnlyToUnix(endDate);

		String tickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		StockInfo result = new StockInfo();
		HttpClient client = new HttpClient();

		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + tickerExt + "?interval=1d&period1=" + startTime + "&period2=" + endTime;

		System.Console.WriteLine("URL: " + url);

		HttpResponseMessage stockHistoryRes = await client.GetAsync(url);
		String stockHistoryCsv = await stockHistoryRes.Content.ReadAsStringAsync();

		System.Console.WriteLine(stockHistoryCsv);
		String[] dataLines = stockHistoryCsv.Replace("\r", "").Split("\n");
		System.Console.WriteLine(dataLines.Length);

		for (int i = 1; i < dataLines.Length; i++)
		{
			String[] data = dataLines[i].Split(",");
			using (SqlConnection connection = Database.createConnection())
			{
				String query = "INSERT INTO StockPrices VALUES (@ticker, @exchange, @date, @open_price, @high_price, @low_price, @close_price, @volume)";
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@ticker", ticker);
				command.Parameters.AddWithValue("@exchange", exchange);
				command.Parameters.AddWithValue("@date", data[0]);
				command.Parameters.AddWithValue("@open_price", Decimal.Parse(data[1]));
				command.Parameters.AddWithValue("@high_price", Decimal.Parse(data[2]));
				command.Parameters.AddWithValue("@low_price", Decimal.Parse(data[3]));
				command.Parameters.AddWithValue("@close_price", Decimal.Parse(data[4]));
				command.Parameters.AddWithValue("@volume", int.Parse(data[6]));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					System.Console.WriteLine(e);
				}
			}
		}
	}
}