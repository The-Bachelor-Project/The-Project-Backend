using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public async Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		SqlConnection connection = (new Database.Connection()).Create();
		String getTrackingDateQuery = "SELECT start_tracking_date, end_tracking_date FROM Currencies WHERE code = @currency";
		SqlCommand command = new SqlCommand(getTrackingDateQuery, connection);
		command.Parameters.AddWithValue("@currency", currency);
		SqlDataReader reader = command.ExecuteReader();

		if (reader.Read())
		{
			DateOnly startTrackingDate;
			DateOnly endTrackingDate;
			try
			{
				startTrackingDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]);
				endTrackingDate = DateOnly.FromDateTime((DateTime)reader["end_tracking_date"]);
			}
			catch (Exception)
			{

				Data.CurrencyHistory fromYahoo = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), endDate);
				SaveCurrencyHistory(fromYahoo, true, true);
				reader.Close();
				return fromYahoo;

			}

			if (startDate < startTrackingDate)
			{

				Data.CurrencyHistory fromYahooBefore = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), startTrackingDate.AddDays(-1));

				SaveCurrencyHistory(fromYahooBefore, true, false);
			}
			if (endDate > endTrackingDate)
			{
				Data.CurrencyHistory fromYahooAfter = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, endTrackingDate.AddDays(1), endDate);
				SaveCurrencyHistory(fromYahooAfter, false, true);
			}
		}
		reader.Close();
		return await (new Data.Fetcher.DatabaseFetcher.CurrencyFetcher()).GetHistory(currency, startDate, endDate);
	}

	private void SaveCurrencyHistory(Data.CurrencyHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
		System.Console.WriteLine(history.history.Count);
		if (history.history.Count == 0)
			return;
		String insertIntoCurrencyRatesQuery = "EXEC BulkJsonCurrencyRates @CurrencyRatesBulk, @Code";
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand(insertIntoCurrencyRatesQuery, connection);
		command.Parameters.AddWithValue("@CurrencyRatesBulk", JsonConvert.SerializeObject(history.history));
		command.Parameters.AddWithValue("@Code", history.currency);
		command.ExecuteNonQuery();

		if (updateStartTrackingDate)
		{
			String updateStartTrackingDateQuery = "UPDATE Currencies SET start_tracking_date = @start_tracking_date WHERE code = @code";
			command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@code", history.currency);
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.First().date));
			command.ExecuteNonQuery();
		}
		if (updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Currencies SET end_tracking_date = @end_tracking_date WHERE code = @code";
			command = new SqlCommand(updateEndTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@code", history.currency);
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.Last().date));
			command.ExecuteNonQuery();
		}
	}
}