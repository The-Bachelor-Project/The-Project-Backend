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
			Result.History = Result.History.Append(new Data.CurrencyHistoryData(
				DateOnly.FromDateTime((DateTime)reader["end_date"]),
				Decimal.Parse("" + reader["open_price"].ToString()),
				Decimal.Parse("" + reader["high_price"].ToString()),
				Decimal.Parse("" + reader["low_price"].ToString()),
				Decimal.Parse("" + reader["close_price"].ToString())
			)).ToArray();
		}


		Result.StartDate = Result.History.First().Date;
		Result.EndDate = Result.History.Last().Date;

		return Task.FromResult(Result);
	}
}