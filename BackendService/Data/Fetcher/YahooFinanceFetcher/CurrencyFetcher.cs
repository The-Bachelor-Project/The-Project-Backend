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
		Boolean isGBX = false;
		if (currency.ToUpper() == "GBX")
		{
			currency = "GBP";
			isGBX = true;
		}
		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + currency + "USD=X" + "?interval=1d&period1=" + startTime + "&period2=" + endTime;
		System.Console.WriteLine(url);
		HttpResponseMessage stockHistoryRes = await client.GetAsync(url);
		String stockHistoryCsv = await stockHistoryRes.Content.ReadAsStringAsync();

		String[] dataLines = stockHistoryCsv.Replace("\r", "").Split("\n");

		Data.CurrencyHistory? result = null;

		if (isGBX)
		{
			result = new Data.CurrencyHistory("GBX", startDate, endDate, "daily");
			for (int i = 1; i < dataLines.Length; i++)
			{
				String[] data = dataLines[i].Split(",");
				if (data[1] == "null")
				{
					Data.DatePrice temp = new Data.DatePrice(DateOnly.Parse(data[0]), result.history.Last().openPrice, result.history.Last().highPrice, result.history.Last().lowPrice, result.history.Last().closePrice);
					temp.openPrice.amount /= 100;
					temp.highPrice.amount /= 100;
					temp.lowPrice.amount /= 100;
					temp.closePrice.amount /= 100;
					result.history.Add(temp);
				}
				else
				{
					Data.DatePrice temp = new Data.DatePrice(DateOnly.Parse(data[0]), new Data.Money(Decimal.Parse(data[1]), Data.Money.DEFAULT_CURRENCY), new Data.Money(Decimal.Parse(data[2]), Data.Money.DEFAULT_CURRENCY), new Data.Money(Decimal.Parse(data[3]), Data.Money.DEFAULT_CURRENCY), new Data.Money(Decimal.Parse(data[4]), Data.Money.DEFAULT_CURRENCY));
					temp.openPrice.amount /= 100;
					temp.highPrice.amount /= 100;
					temp.lowPrice.amount /= 100;
					temp.closePrice.amount /= 100;
					result.history.Add(temp);
				}
			}
		}
		else
		{
			result = new Data.CurrencyHistory(currency, startDate, endDate, "daily");
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
					result.history.Add(new Data.DatePrice(DateOnly.Parse(data[0]), new Data.Money(Decimal.Parse(data[1]), Data.Money.DEFAULT_CURRENCY), new Data.Money(Decimal.Parse(data[2]), Data.Money.DEFAULT_CURRENCY), new Data.Money(Decimal.Parse(data[3]), Data.Money.DEFAULT_CURRENCY), new Data.Money(Decimal.Parse(data[4]), Data.Money.DEFAULT_CURRENCY)));
				}
			}
		}

		return result;
	}
}