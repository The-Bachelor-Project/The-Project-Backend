using System.Data.SqlClient;

namespace DatabaseService;

class StockHistory
{
	public static async Task<Data.StockHistory> Get(Data.StockHistory history)
	{
		SqlConnection connection = Database.createConnection();
		String getTrackingDateQuery = "SELECT start_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		SqlCommand command = new SqlCommand(getTrackingDateQuery, connection);
		command.Parameters.AddWithValue("@ticker", history.ticker);
		command.Parameters.AddWithValue("@exchange", history.exchange);
		SqlDataReader reader = command.ExecuteReader();
		if (reader.Read())
		{
			DateOnly trackingDate;
			try
			{
				trackingDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]);
			}
			catch (Exception)
			{
				trackingDate = DateOnly.FromDateTime(DateTime.Now);
			}

			reader.Close();

			DateOnly startDate = DateOnly.Parse(history.start_date);
			if (startDate < trackingDate)
			{
				await StockPricesUpdater.update(history.ticker, history.exchange, startDate);
			}


			DateOnly endDate = history.end_date == "" ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.Parse(history.end_date);
			System.Console.WriteLine(endDate);
			String getStockHistoryQuery = "SELECT * FROM GetStockPrices(@ticker, @exchange, @interval, @start_date, @end_date)";
			command = new SqlCommand(getStockHistoryQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.ticker);
			command.Parameters.AddWithValue("@exchange", history.exchange);
			command.Parameters.AddWithValue("@interval", history.interval);
			command.Parameters.AddWithValue("@start_date", history.start_date);
			command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
			reader = command.ExecuteReader();
			while (reader.Read())
			{
				history.history = history.history.Append(new Data.StockHistoryData(Tools.TimeConverter.dateOnlyToString(DateOnly.FromDateTime((DateTime)reader["end_date"])), Decimal.Parse("" + reader["open_price"].ToString()), Decimal.Parse("" + reader["high_price"].ToString()), Decimal.Parse("" + reader["low_price"].ToString()), Decimal.Parse("" + reader["close_price"].ToString()))).ToArray();
			}
			return history;
		}
		else
		{
			throw new Exception();
		}
	}
}