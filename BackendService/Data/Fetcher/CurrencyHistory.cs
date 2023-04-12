using System.Data.SqlClient;
using Data.Interfaces;

namespace Data.Fetcher;

public class CurrencyHistory : ICurrencyHistory
{
	public CurrencyHistory()
	{
	}

	public async Task<Data.CurrencyHistory> Usd(string currency, DateOnly startDate, DateOnly endDate)
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
				Data.CurrencyHistory FromYahoo = await (new Data.YahooFinance.CurrencyHistory()).Usd(currency, startDate.AddDays(-7), endDate);
				//TODO save to DB from here instead of on yahoo implementation
				return FromYahoo;
			}

			reader.Close();

			if (startDate < StartTrackingDate)
			{
				Data.CurrencyHistory FromYahooBefore = await (new Data.YahooFinance.CurrencyHistory()).Usd(currency, startDate.AddDays(-7), StartTrackingDate.AddDays(-1));
				//SaveStockHistory(FromYahooBefore, true, false);
			}
			if (endDate > EndTrackingDate)
			{
				Data.CurrencyHistory FromYahooAfter = await (new Data.YahooFinance.CurrencyHistory()).Usd(currency, EndTrackingDate.AddDays(1), endDate);
				//SaveStockHistory(FromYahooAfter, false, true);
			}
		}


		return await (new Data.Database.CurrencyHistory()).Usd(currency, startDate, endDate);
	}
}