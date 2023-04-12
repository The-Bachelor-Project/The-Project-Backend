using System.Data.SqlClient;

namespace DatabaseService;

class CurrencyRatesHistory
{
	public static async Task<Data.CurrencyHistory> Get(Data.CurrencyHistory history)
	{
		SqlConnection Connection = Database.createConnection();
		String GetTrackingDateQuery = "SELECT start_tracking_date FROM Currencies WHERE code = @currency";
		SqlCommand Command = new SqlCommand(GetTrackingDateQuery, Connection);
		Command.Parameters.AddWithValue("@currency", history.Currency);
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

			//DateOnly StartDate = DateOnly.Parse(history.StartDate);
			//if (StartDate < TrackingDate)
			//{
			//	await CurrencyRatesUpdater.Update(history.Currency, StartDate);
			//}
			//
			//
			//DateOnly EndDate = history.EndDate == "" ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.Parse(history.EndDate);
			//System.Console.WriteLine(EndDate);
			//String GetCurrencyHistoryQuery = "SELECT * FROM GetCurrencyRates(@currency, @interval, @start_date, @end_date)";
			//Command = new SqlCommand(GetCurrencyHistoryQuery, Connection);
			//Command.Parameters.AddWithValue("@currency", history.Currency);
			//Command.Parameters.AddWithValue("@interval", history.Interval);
			//Command.Parameters.AddWithValue("@start_date", history.StartDate);
			//Command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(EndDate));
			//Reader = Command.ExecuteReader();
			//while (Reader.Read())
			//{
			//	history.History = history.History.Append(new Data.CurrencyHistoryData(Tools.TimeConverter.dateOnlyToString(DateOnly.FromDateTime((DateTime)Reader["end_date"])), Decimal.Parse("" + Reader["open_price"].ToString()), Decimal.Parse("" + Reader["high_price"].ToString()), Decimal.Parse("" + Reader["low_price"].ToString()), Decimal.Parse("" + Reader["close_price"].ToString()))).ToArray();
			//}
			//return history;
			throw new NotImplementedException();
		}
		else
		{
			throw new Exception();
		}
	}
}