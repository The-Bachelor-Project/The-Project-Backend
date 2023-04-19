using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher.YahooFinanceFetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public async Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		int StartTime = Tools.TimeConverter.dateOnlyToUnix(startDate);
		int EndTime = Tools.TimeConverter.dateOnlyToUnix(endDate);

		HttpClient Client = new HttpClient();

		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + currency + "USD=X" + "?interval=1d&period1=" + StartTime + "&period2=" + EndTime;
		HttpResponseMessage StockHistoryRes = await Client.GetAsync(url);
		String StockHistoryCsv = await StockHistoryRes.Content.ReadAsStringAsync();

		String[] DataLines = StockHistoryCsv.Replace("\r", "").Split("\n");

		Data.CurrencyHistory Result = new Data.CurrencyHistory(currency, startDate, endDate, "daily");
		for (int i = 1; i < DataLines.Length; i++)
		{
			String[] Data = DataLines[i].Split(",");
			if (Data[1] == "null")
			{
				Data.DatePrice temp = new Data.DatePrice(DateOnly.Parse(Data[0]), Result.History.Last().openPrice, Result.History.Last().highPrice, Result.History.Last().lowPrice, Result.History.Last().closePrice);
				Result.History.Add(temp);
			}
			else
			{
				Result.History.Add(new Data.DatePrice(DateOnly.Parse(Data[0]), new StockApp.Money(Decimal.Parse(Data[1])), new StockApp.Money(Decimal.Parse(Data[2])), new StockApp.Money(Decimal.Parse(Data[3])), new StockApp.Money(Decimal.Parse(Data[4]))));
			}
		}
		return Result;
	}
}