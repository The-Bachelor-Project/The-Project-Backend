using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher;

public class CurrencyFetcher : ICurrencyFetcher
{


	public async Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		String getTrackingDateQuery = "SELECT start_tracking_date, end_tracking_date FROM Currencies WHERE code = @currency";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@currency", currency);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getTrackingDateQuery, parameters);
		if (data != null)
		{
			DateOnly startTrackingDate;
			DateOnly endTrackingDate;
			try
			{
				startTrackingDate = DateOnly.FromDateTime((DateTime)data["start_tracking_date"]);
				endTrackingDate = DateOnly.FromDateTime((DateTime)data["end_tracking_date"]);
			}
			catch (Exception)
			{
				Data.CurrencyHistory fromYahoo = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), endDate);
				SaveCurrencyHistory(fromYahoo, true, true);
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
		return await (new Data.Fetcher.DatabaseFetcher.CurrencyFetcher()).GetHistory(currency, startDate, endDate);
	}

	private void SaveCurrencyHistory(Data.CurrencyHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
		System.Console.WriteLine(history.history.Count);
		if (history.history.Count == 0)
			return;
		String insertIntoCurrencyRatesQuery = "EXEC BulkJsonCurrencyRates @CurrencyRatesBulk, @Code";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(insertIntoCurrencyRatesQuery, connection);
		command.Parameters.AddWithValue("@CurrencyRatesBulk", JsonConvert.SerializeObject(history.history));
		command.Parameters.AddWithValue("@Code", history.currency);
		try
		{
			command.ExecuteNonQuery();

		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new DatabaseException("There was a problem when adding currency rates to the database");
		}

		if (updateStartTrackingDate)
		{
			String updateStartTrackingDateQuery = "UPDATE Currencies SET start_tracking_date = @start_tracking_date WHERE code = @code";
			command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@code", history.currency);
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.First().date));
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				throw new DatabaseException("There was a problem when updating the start tracking date of the currency");
			}
		}
		if (updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Currencies SET end_tracking_date = @end_tracking_date WHERE code = @code";
			command = new SqlCommand(updateEndTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@code", history.currency);
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.Last().date));
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				throw new DatabaseException("There was a problem when updating the end tracking date of the currency");
			}
		}
	}
}