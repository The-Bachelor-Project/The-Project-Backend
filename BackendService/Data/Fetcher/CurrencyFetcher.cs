using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public async Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		SqlConnection Connection = (new Database.Connection()).Create();
		String GetTrackingDateQuery = "SELECT start_tracking_date, end_tracking_date FROM Currencies WHERE code = @currency";
		SqlCommand Command = new SqlCommand(GetTrackingDateQuery, Connection);
		Command.Parameters.AddWithValue("@currency", currency);
		SqlDataReader reader = Command.ExecuteReader();

		if (reader.Read())
		{
			DateOnly StartTrackingDate;
			DateOnly EndTrackingDate;
			try
			{
				StartTrackingDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]);
				EndTrackingDate = DateOnly.FromDateTime((DateTime)reader["end_tracking_date"]);
			}
			catch (Exception)
			{
				Data.CurrencyHistory FromYahoo = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), endDate);
				SaveCurrencyHistory(FromYahoo, true, true);
				return FromYahoo;
			}

			reader.Close();

			if (startDate < StartTrackingDate)
			{
				Data.CurrencyHistory FromYahooBefore = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), StartTrackingDate.AddDays(-1));
				SaveCurrencyHistory(FromYahooBefore, true, false);
			}
			if (endDate > EndTrackingDate)
			{
				Data.CurrencyHistory FromYahooAfter = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, EndTrackingDate.AddDays(1), endDate);
				SaveCurrencyHistory(FromYahooAfter, false, true);
			}
		}


		return await (new Data.Fetcher.DatabaseFetcher.CurrencyFetcher()).GetHistory(currency, startDate, endDate);
	}

	private void SaveCurrencyHistory(Data.CurrencyHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
		System.Console.WriteLine(history.History.Count);
		if (history.History.Count == 0)
			return;
		String InsertIntoCurrencyRatesQuery = "EXEC BulkJsonCurrencyRates @CurrencyRatesBulk, @Code";
		SqlConnection Connection = new Data.Database.Connection().Create();
		SqlCommand Command = new SqlCommand(InsertIntoCurrencyRatesQuery, Connection);
		Command.Parameters.AddWithValue("@CurrencyRatesBulk", JsonConvert.SerializeObject(history.History));
		Command.Parameters.AddWithValue("@Code", history.Currency);
		Command.ExecuteNonQuery();

		if (updateStartTrackingDate)
		{
			String updateStartTrackingDateQuery = "UPDATE Currencies SET start_tracking_date = @start_tracking_date WHERE code = @code";
			Command = new SqlCommand(updateStartTrackingDateQuery, Connection);
			Command.Parameters.AddWithValue("@code", history.Currency);
			Command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.First().date));
			Command.ExecuteNonQuery();
		}
		if (updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Currencies SET end_tracking_date = @end_tracking_date WHERE code = @code";
			Command = new SqlCommand(updateEndTrackingDateQuery, Connection);
			Command.Parameters.AddWithValue("@code", history.Currency);
			Command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.Last().date));
			Command.ExecuteNonQuery();
		}
	}
}