using System.Data.SqlClient;

namespace DatabaseService;

class CurrencyRatesHistory
{
	public static async Task<Data.CurrencyHistory> Get(Data.CurrencyHistory History)
	{
		SqlConnection Connection = Database.createConnection();
		String GetTrackingDateQuery = "SELECT start_tracking_date FROM Currencies WHERE code = @currency";
		SqlCommand Command = new SqlCommand(GetTrackingDateQuery, Connection);
		Command.Parameters.AddWithValue("@currency", History.Currency);
		SqlDataReader Reader = Command.ExecuteReader();
		if (Reader.Read())
		{
			DateOnly TrackingDate;
			try
			{
				TrackingDate = DateOnly.FromDateTime((DateTime)Reader["start_tracking_date"]);
			}
			catch (Exception)
			{
				TrackingDate = DateOnly.FromDateTime(DateTime.Now);
			}

			Reader.Close();

			DateOnly StartDate = DateOnly.Parse(History.StartDate);
			if (StartDate < TrackingDate)
			{
				await CurrencyRatesUpdater.Update(History.Currency, StartDate);
			}


			DateOnly EndDate = History.EndDate == "" ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.Parse(History.EndDate);
			System.Console.WriteLine(EndDate);
			String GetCurrencyHistoryQuery = "SELECT * FROM GetCurrencyRates(@currency, @interval, @start_date, @end_date)";
			Command = new SqlCommand(GetCurrencyHistoryQuery, Connection);
			Command.Parameters.AddWithValue("@currency", History.Currency);
			Command.Parameters.AddWithValue("@interval", History.Interval);
			Command.Parameters.AddWithValue("@start_date", History.StartDate);
			Command.Parameters.AddWithValue("@end_date", TimeConverter.dateOnlyToString(EndDate));
			Reader = Command.ExecuteReader();
			while (Reader.Read())
			{
				History.History = History.History.Append(new Data.CurrencyHistoryData(TimeConverter.dateOnlyToString(DateOnly.FromDateTime((DateTime)Reader["end_date"])), Decimal.Parse("" + Reader["open_price"].ToString()), Decimal.Parse("" + Reader["high_price"].ToString()), Decimal.Parse("" + Reader["low_price"].ToString()), Decimal.Parse("" + Reader["close_price"].ToString()))).ToArray();
			}
			return History;
		}
		else
		{
			throw new Exception();
		}
	}
}