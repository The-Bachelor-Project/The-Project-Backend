using Data.Interfaces;

namespace Data.YahooFinance;

class CurrencyHistory : ICurrencyHistory
{
	public async Task<Data.CurrencyHistory> Usd(String currency, DateOnly startDate, DateOnly endDate)
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
			Result.History = Result.History.Append(new Data.CurrencyHistoryData(DateOnly.Parse(Data[0]), Decimal.Parse(Data[1]), Decimal.Parse(Data[2]), Decimal.Parse(Data[3]), Decimal.Parse(Data[4]))).ToArray();
		}
		return Result;
	}
}