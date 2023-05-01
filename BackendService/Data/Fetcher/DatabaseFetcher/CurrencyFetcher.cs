using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher.DatabaseFetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		System.Console.WriteLine(currency);
		SqlConnection connection = new Database.Connection().Create();
		String getCurrencyHistoryQuery = "SELECT * FROM GetCurrencyRates(@currency, @interval, @start_date, @end_date)";
		SqlCommand command = new SqlCommand(getCurrencyHistoryQuery, connection);
		command.Parameters.AddWithValue("@currency", currency);
		command.Parameters.AddWithValue("@start_date", Tools.TimeConverter.dateOnlyToString(startDate));
		command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
		command.Parameters.AddWithValue("@interval", "daily");
		SqlDataReader reader = command.ExecuteReader();

		Data.CurrencyHistory result = new Data.CurrencyHistory(currency, startDate, endDate, "daily");
		while (reader.Read())
		{
			result.history.Add(new Data.DatePrice(
				DateOnly.FromDateTime(DateTime.Parse(reader["end_date"].ToString()!)),
				new Data.Money(Decimal.Parse("" + reader["open_price"].ToString()), Data.Money.DEFAULT_CURRENCY),
				new Data.Money(Decimal.Parse("" + reader["high_price"].ToString()), Data.Money.DEFAULT_CURRENCY),
				new Data.Money(Decimal.Parse("" + reader["low_price"].ToString()), Data.Money.DEFAULT_CURRENCY),
				new Data.Money(Decimal.Parse("" + reader["close_price"].ToString()), Data.Money.DEFAULT_CURRENCY)
			));
		}
		reader.Close();

		result.startDate = result.history.First().date;
		result.endDate = result.history.Last().date;

		return Task.FromResult(result);
	}
}