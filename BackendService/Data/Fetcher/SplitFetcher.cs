namespace Data.Fetcher.YahooFinanceFetcher;

public class SplitFetcher
{
	public async Task<Dictionary<DateOnly, Dictionary<int, int>>> GetSplits(string ticker, string exchange, DateOnly startDate, DateOnly endDate)
	{
		int startTime = Tools.TimeConverter.DateOnlyToUnix(startDate);
		int endTime = Tools.TimeConverter.DateOnlyToUnix(endDate);
		String tickerExt = YfTranslator.GetYfSymbol(ticker, exchange);
		HttpClient client = new HttpClient();
		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + tickerExt + "?period1=" + startTime + "&period2=" + endTime + "&interval=1d&events=split";
		HttpResponseMessage stockSplitsHis = await client.GetAsync(url);
		if (stockSplitsHis.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return new Dictionary<DateOnly, Dictionary<int, int>>();
		}

		String stockSplitsHisCsv = await stockSplitsHis.Content.ReadAsStringAsync();
		String[] dataLines = stockSplitsHisCsv.Replace("\r", "").Split("\n");
		Dictionary<DateOnly, Dictionary<int, int>> result = new Dictionary<DateOnly, Dictionary<int, int>>();
		if (dataLines.Count() == 1)
		{
			return result;
		}
		for (int i = 1; i < dataLines.Length; i++)
		{
			try
			{
				String[] data = dataLines[i].Split(",");
				DateOnly date = DateOnly.Parse(data[0]);
				int ratioOut = int.Parse(data[1].Split(":")[0]);
				int ratioIn = int.Parse(data[1].Split(":")[1]);
				result.Add(date, new Dictionary<int, int>(){
					{ratioOut, ratioIn}
				});
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				throw new StatusCodeException(500, "There was a problem when getting splits: " + e);
			}
		}
		return result;
	}
}
