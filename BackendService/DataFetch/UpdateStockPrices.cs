using System.Data.SqlClient;
using BackendService;

class StockPricesUpdater
{
	public static async Task update(String ticker, String exchange, DateOnly startDate)
	{

		using (SqlConnection connection = DatabaseService.Database.createConnection())
		{
			DateOnly endDate = new DateOnly();
			String getEndDateQuery = "SELECT start_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
			SqlCommand command = new SqlCommand(getEndDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", ticker);
			command.Parameters.AddWithValue("@exchange", exchange);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				try
				{
					endDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]).AddDays(-1);
					reader.Close();
					await _update(ticker, exchange, startDate, endDate);
				}
				catch (Exception)
				{
					reader.Close();
					endDate = DateOnly.FromDateTime(DateTime.Now);
					endDate = await _update(ticker, exchange, startDate, endDate);

					String updateEndTrackingDateQuery = "UPDATE Stocks SET end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
					command = new SqlCommand(updateEndTrackingDateQuery, connection);
					command.Parameters.AddWithValue("@end_tracking_date", TimeConverter.dateOnlyToString(endDate));
					command.Parameters.AddWithValue("@ticker", ticker);
					command.Parameters.AddWithValue("@exchange", exchange);
					command.ExecuteNonQuery();
				}
			}
			else
			{
				//TODO make a better exception
				throw new Exception();
			}



			String updateStartTrackingDateQuery = "UPDATE Stocks SET start_tracking_date = @start_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@start_tracking_date", TimeConverter.dateOnlyToString(startDate));
			command.Parameters.AddWithValue("@ticker", ticker);
			command.Parameters.AddWithValue("@exchange", exchange);
			command.ExecuteNonQuery();
		}
	}

	public static async Task update(String ticker, String exchange)
	{
		using (SqlConnection connection = DatabaseService.Database.createConnection())
		{
			DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
			DateOnly startDate = new DateOnly();
			String getStartDateQuery = "SELECT end_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
			SqlCommand command = new SqlCommand(getStartDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", ticker);
			command.Parameters.AddWithValue("@exchange", exchange);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				try
				{
					startDate = DateOnly.Parse("" + reader["end_tracking_date"].ToString()).AddDays(-1);
					reader.Close();
				}
				catch (Exception)
				{
					reader.Close();
					//TODO make a better exception
					throw new Exception();
				}
			}
			else
			{
				//TODO make a better exception
				throw new Exception();
			}
			endDate = await _update(ticker, exchange, startDate, endDate);


			String updateStartTrackingDateQuery = "UPDATE Stocks SET end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@end_tracking_date", endDate);
			command.Parameters.AddWithValue("@ticker", ticker);
			command.Parameters.AddWithValue("@exchange", exchange);
			command.ExecuteNonQuery();
		}
	}

	private static async Task<DateOnly> _update(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
	{
		int startTime = TimeConverter.dateOnlyToUnix(startDate);
		int endTime = TimeConverter.dateOnlyToUnix(endDate);


		String[] dataLines = await DataFetcher.stockHistory(ticker, exchange, startDate, endDate);

		// await CurrencyConversion.GetMissingRatesAsync(ticker, exchange, startDate, endDate);
		String insertIntoStockPricesQuery = "INSERT INTO StockPrices VALUES (@ticker, @exchange, @date, @open_price, @high_price, @low_price, @close_price, @volume)";
		String lastDate = "";
		for (int i = 1; i < dataLines.Length; i++)
		{
			String[] data = dataLines[i].Split(",");
			lastDate = data[0];
			//TODO: Add currency conversion
			using (SqlConnection connection = DatabaseService.Database.createConnection())
			{
				//TODO Look into using a BULK INSERT query
				SqlCommand command = new SqlCommand(insertIntoStockPricesQuery, connection);
				command.Parameters.AddWithValue("@ticker", ticker);
				command.Parameters.AddWithValue("@exchange", exchange);
				command.Parameters.AddWithValue("@date", data[0]);
				command.Parameters.AddWithValue("@open_price", Decimal.Parse(data[1]));
				command.Parameters.AddWithValue("@high_price", Decimal.Parse(data[2]));
				command.Parameters.AddWithValue("@low_price", Decimal.Parse(data[3]));
				command.Parameters.AddWithValue("@close_price", Decimal.Parse(data[4]));
				command.Parameters.AddWithValue("@volume", int.Parse(data[6]));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
			}
		}
		return DateOnly.Parse(lastDate);
	}
}