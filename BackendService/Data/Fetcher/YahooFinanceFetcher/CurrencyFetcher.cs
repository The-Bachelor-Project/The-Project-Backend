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
				CurrencyHistoryData temp = new Data.CurrencyHistoryData(DateOnly.Parse(Data[0]), Result.History.Last().OpenPrice, Result.History.Last().HighPrice, Result.History.Last().LowPrice, Result.History.Last().ClosePrice);
				Result.History = Result.History.Append(temp).ToArray();
			}
			else
			{
				Result.History = Result.History.Append(new Data.CurrencyHistoryData(DateOnly.Parse(Data[0]), Decimal.Parse(Data[1]), Decimal.Parse(Data[2]), Decimal.Parse(Data[3]), Decimal.Parse(Data[4]))).ToArray();
			}
		}
		return Result;
	}
}