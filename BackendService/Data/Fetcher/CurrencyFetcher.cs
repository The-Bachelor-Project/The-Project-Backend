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
				if (currency.ToUpper() == "GBX")
				{
					Data.CurrencyHistory gbpHistory = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory("GBP", startDate.AddDays(-7), endDate);
					Data.CurrencyHistory fromYahoo = ConvertToGBX(gbpHistory, startDate, endDate);
					SaveCurrencyHistory(fromYahoo, true, true);
					SaveCurrencyHistory(gbpHistory, true, true);
					return fromYahoo;
				}
				else
				{
					Data.CurrencyHistory fromYahoo = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), endDate);
					SaveCurrencyHistory(fromYahoo, true, true);
					return fromYahoo;
				}

			}

			reader.Close();

			if (startDate < startTrackingDate)
			{
				Data.CurrencyHistory? fromYahooBefore = null;
				if (currency.ToUpper() == "GBX")
				{
					Data.CurrencyHistory gbpHistory = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory("GBP", startDate.AddDays(-7), startTrackingDate.AddDays(-1));
					fromYahooBefore = ConvertToGBX(gbpHistory, startDate, endDate);
					SaveCurrencyHistory(gbpHistory, true, false);
				}
				else
				{
					fromYahooBefore = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), startTrackingDate.AddDays(-1));
				}
				SaveCurrencyHistory(fromYahooBefore, true, false);
			}
			if (endDate > endTrackingDate)
			{
				Data.CurrencyHistory? fromYahooAfter = null;
				if (currency.ToUpper() == "GBX")
				{
					Data.CurrencyHistory gbpHistory = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory("GBP", endTrackingDate.AddDays(1), endDate);
					fromYahooAfter = ConvertToGBX(gbpHistory, startDate, endDate);
					SaveCurrencyHistory(gbpHistory, false, true);
				}
				else
				{
					fromYahooAfter = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, endTrackingDate.AddDays(1), endDate);
				}

				SaveCurrencyHistory(fromYahooAfter, false, true);
			}
		}

		if (currency.ToUpper() == "GBX")
		{
			Data.CurrencyHistory gbpHistory = await (new Data.Fetcher.DatabaseFetcher.CurrencyFetcher()).GetHistory("GBP", startDate, endDate);
			return ConvertToGBX(gbpHistory, startDate, endDate); ;
		}
		else
		{
			return await (new Data.Fetcher.DatabaseFetcher.CurrencyFetcher()).GetHistory(currency, startDate, endDate);
		}
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

	private Data.CurrencyHistory ConvertToGBX(Data.CurrencyHistory history, DateOnly startDate, DateOnly endDate)
	{
		String currency = "GBX";
		history.currency = currency;
		for (int i = 0; i < history.history.Count; i++)
		{
			history.history[i].openPrice.amount = history.history[i].openPrice.amount / 100;
			history.history[i].openPrice.currency = currency;
			history.history[i].closePrice.amount = history.history[i].closePrice.amount / 100;
			history.history[i].closePrice.currency = currency;
			history.history[i].highPrice.amount = history.history[i].highPrice.amount / 100;
			history.history[i].highPrice.currency = currency;
			history.history[i].lowPrice.amount = history.history[i].lowPrice.amount / 100;
			history.history[i].lowPrice.currency = currency;
		}
		return history;
	}
}