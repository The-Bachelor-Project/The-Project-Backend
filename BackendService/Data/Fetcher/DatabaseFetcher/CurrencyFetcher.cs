using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher.DatabaseFetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		SqlConnection Connection = new Database.Connection().Create();
		String GetCurrencyHistoryQuery = "SELECT * FROM GetCurrencyRates(@currency, @interval, @start_date, @end_date)";
		SqlCommand Command = new SqlCommand(GetCurrencyHistoryQuery, Connection);
		Command.Parameters.AddWithValue("@currency", currency);
		Command.Parameters.AddWithValue("@start_date", Tools.TimeConverter.dateOnlyToString(startDate));
		Command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
		Command.Parameters.AddWithValue("@interval", "daily");
		SqlDataReader reader = Command.ExecuteReader();

		Data.CurrencyHistory Result = new Data.CurrencyHistory(currency, startDate, endDate, "daily");
		while (reader.Read())
		{
			Result.History.Add(new Data.DatePrice(
				DateOnly.FromDateTime((DateTime)reader["end_date"]),
				new StockApp.Money(Decimal.Parse("" + reader["open_price"].ToString())),
				new StockApp.Money(Decimal.Parse("" + reader["high_price"].ToString())),
				new StockApp.Money(Decimal.Parse("" + reader["low_price"].ToString())),
				new StockApp.Money(Decimal.Parse("" + reader["close_price"].ToString()))
			));
		}


		Result.StartDate = Result.History.First().date;
		Result.EndDate = Result.History.Last().date;

		return Task.FromResult(Result);
	}
}