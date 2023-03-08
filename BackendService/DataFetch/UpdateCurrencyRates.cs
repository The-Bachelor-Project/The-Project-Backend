using System.Data.SqlClient;
using BackendService;

class CurrencyRatesUpdater
{
	public static async Task Update(String currency, DateOnly startDate)
	{

		using (SqlConnection Connection = DatabaseService.Database.createConnection())
		{
			DateOnly EndDate = new DateOnly();
			String GetEndDateQuery = "SELECT start_tracking_date FROM Currencies WHERE code = @currency";
			SqlCommand Command = new SqlCommand(GetEndDateQuery, Connection);
			Command.Parameters.AddWithValue("@currency", currency);
			SqlDataReader Reader = Command.ExecuteReader();
			if (Reader.Read())
			{
				try
				{
					EndDate = DateOnly.FromDateTime((DateTime)Reader["start_tracking_date"]).AddDays(-1);
					Reader.Close();
					await _Update(currency, startDate, EndDate);
				}
				catch (Exception)
				{
					Reader.Close();
					EndDate = DateOnly.FromDateTime(DateTime.Now);
					EndDate = await _Update(currency, startDate, EndDate);

					String UpdateEndTrackingDateQuery = "UPDATE Currencies SET end_tracking_date = @end_tracking_date WHERE code = @currency";
					Command = new SqlCommand(UpdateEndTrackingDateQuery, Connection);
					Command.Parameters.AddWithValue("@end_tracking_date", TimeConverter.dateOnlyToString(EndDate));
					Command.Parameters.AddWithValue("@currency", currency);
					Command.ExecuteNonQuery();
				}
			}
			else
			{
				//TODO make a better exception
				throw new Exception();
			}



			String UpdateStartTrackingDateQuery = "UPDATE Currencies SET start_tracking_date = @start_tracking_date WHERE code = @currency";
			Command = new SqlCommand(UpdateStartTrackingDateQuery, Connection);
			Command.Parameters.AddWithValue("@start_tracking_date", TimeConverter.dateOnlyToString(startDate));
			Command.Parameters.AddWithValue("@currency", currency);
			Command.ExecuteNonQuery();
		}
	}

	public static async Task Update(String currency)
	{
		using (SqlConnection Connection = DatabaseService.Database.createConnection())
		{
			DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
			DateOnly startDate = new DateOnly();
			String GetStartDateQuery = "SELECT end_tracking_date FROM Currencies WHERE code = @currency";
			SqlCommand Command = new SqlCommand(GetStartDateQuery, Connection);
			Command.Parameters.AddWithValue("@currency", currency);
			SqlDataReader Reader = Command.ExecuteReader();
			if (Reader.Read())
			{
				try
				{
					startDate = DateOnly.Parse("" + Reader["end_tracking_date"].ToString()).AddDays(-1);
					Reader.Close();
				}
				catch (Exception)
				{
					Reader.Close();
					//TODO make a better exception
					throw new Exception();
				}
			}
			else
			{
				//TODO make a better exception
				throw new Exception();
			}
			endDate = await _Update(currency, startDate, endDate);


			String updateStartTrackingDateQuery = "UPDATE Currencies SET end_tracking_date = @end_tracking_date WHERE code = @currency";
			Command = new SqlCommand(updateStartTrackingDateQuery, Connection);
			Command.Parameters.AddWithValue("@end_tracking_date", endDate);
			Command.Parameters.AddWithValue("@currency", currency);
			Command.ExecuteNonQuery();
		}
	}

	private static async Task<DateOnly> _Update(String currency, DateOnly startDate, DateOnly endDate)
	{
		int StartTime = TimeConverter.dateOnlyToUnix(startDate);
		int EndTime = TimeConverter.dateOnlyToUnix(endDate);

		String[] DataLines = await DataFetcher.CurrencyHistory(currency, startDate, endDate);

		String InsertIntoCurrencyRatesQuery = "INSERT INTO CurrencyRatesUSD VALUES (@code, @date, @open_price, @high_price, @low_price, @close_price)";
		String LastDate = "";
		for (int i = 1; i < DataLines.Length; i++)
		{
			String[] Data = DataLines[i].Split(",");
			LastDate = Data[0];
			using (SqlConnection Connection = DatabaseService.Database.createConnection())
			{
				//TODO Look into using a BULK INSERT query
				SqlCommand Command = new SqlCommand(InsertIntoCurrencyRatesQuery, Connection);
				Command.Parameters.AddWithValue("@code", currency);
				Command.Parameters.AddWithValue("@date", Data[0]);
				Command.Parameters.AddWithValue("@open_price", Decimal.Parse(Data[1]));
				Command.Parameters.AddWithValue("@high_price", Decimal.Parse(Data[2]));
				Command.Parameters.AddWithValue("@low_price", Decimal.Parse(Data[3]));
				Command.Parameters.AddWithValue("@close_price", Decimal.Parse(Data[4]));
				Command.Parameters.AddWithValue("@volume", int.Parse(Data[6]));
				try
				{
					Command.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
			}
		}
		return DateOnly.Parse(LastDate);
	}
}