using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher.DatabaseFetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		System.Console.WriteLine(currency);
		String getCurrencyHistoryQuery = "SELECT * FROM GetCurrencyRates(@currency, @interval, @start_date, @end_date)";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@currency", currency);
		parameters.Add("@start_date", Tools.TimeConverter.dateOnlyToString(startDate));
		parameters.Add("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
		parameters.Add("@interval", "daily");
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(getCurrencyHistoryQuery, parameters);

		Data.CurrencyHistory result = new Data.CurrencyHistory(currency, startDate, endDate, "daily");
		if (data.Count == 0)
			return Task.FromResult(result);

		foreach (Dictionary<String, object> row in data)
		{
			try
			{
				result.history.Add(new Data.DatePriceOHLC(
					DateOnly.FromDateTime(DateTime.Parse(row["end_date"].ToString()!)),
					new StockApp.Money(Decimal.Parse("" + row["open_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY),
					new StockApp.Money(Decimal.Parse("" + row["high_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY),
					new StockApp.Money(Decimal.Parse("" + row["low_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY),
					new StockApp.Money(Decimal.Parse("" + row["close_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY)
				));
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				continue;
			}

		}
		result.startDate = result.history.First().date;
		result.endDate = result.history.Last().date;
		return Task.FromResult(result);
	}
}