using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher.YahooFinanceFetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public async Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		int startTime = Tools.TimeConverter.dateOnlyToUnix(startDate);
		int endTime = Tools.TimeConverter.dateOnlyToUnix(endDate);

		HttpClient client = new HttpClient();

		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + currency + "USD=X" + "?interval=1d&period1=" + startTime + "&period2=" + endTime;
		System.Console.WriteLine(url);
		HttpResponseMessage stockHistoryRes = await client.GetAsync(url);
		String stockHistoryCsv = await stockHistoryRes.Content.ReadAsStringAsync();

		String[] dataLines = stockHistoryCsv.Replace("\r", "").Split("\n");

		Data.CurrencyHistory result = new Data.CurrencyHistory(currency, startDate, endDate, "daily");
		for (int i = 1; i < dataLines.Length; i++)
		{
			String[] data = dataLines[i].Split(",");
			if (data[1] == "null")
			{
				Data.DatePrice temp = new Data.DatePrice(DateOnly.Parse(data[0]), result.history.Last().openPrice, result.history.Last().highPrice, result.history.Last().lowPrice, result.history.Last().closePrice);
				result.history.Add(temp);
			}
			else
			{
				result.history.Add(new Data.DatePrice(DateOnly.Parse(data[0]), new StockApp.Money(Decimal.Parse(data[1])), new StockApp.Money(Decimal.Parse(data[2])), new StockApp.Money(Decimal.Parse(data[3])), new StockApp.Money(Decimal.Parse(data[4]))));
			}
		}
		return result;
	}
}