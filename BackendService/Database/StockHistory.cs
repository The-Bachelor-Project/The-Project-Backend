using System.Data.SqlClient;

namespace DatabaseService;

class StockHistory
{
	public static async Task<Data.StockHistory> Get(Data.StockHistory history)
	{
		//SqlConnection connection = Database.createConnection();
		//String getTrackingDateQuery = "SELECT start_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		//SqlCommand command = new SqlCommand(getTrackingDateQuery, connection);
		//command.Parameters.AddWithValue("@ticker", history.Ticker);
		//command.Parameters.AddWithValue("@exchange", history.Exchange);
		//SqlDataReader reader = command.ExecuteReader();
		//if (reader.Read())
		//{
		//	DateOnly trackingDate;
		//	try
		//	{
		//		trackingDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]);
		//	}
		//	catch (Exception)
		//	{
		//		trackingDate = DateOnly.FromDateTime(DateTime.Now);
		//	}
//
		//	reader.Close();
//
		//	DateOnly startDate = DateOnly.Parse(history.StartDate);
		//	if (startDate < trackingDate)
		//	{
		//		await StockPricesUpdater.update(history.Ticker, history.Exchange, startDate);
		//	}
//
//
		//	DateOnly endDate = history.EndDate == "" ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.Parse(history.EndDate);
		//	System.Console.WriteLine(endDate);
		//	String getStockHistoryQuery = "SELECT * FROM GetStockPrices(@ticker, @exchange, @interval, @start_date, @end_date)";
		//	command = new SqlCommand(getStockHistoryQuery, connection);
		//	command.Parameters.AddWithValue("@ticker", history.Ticker);
		//	command.Parameters.AddWithValue("@exchange", history.Exchange);
		//	command.Parameters.AddWithValue("@interval", history.Interval);
		//	command.Parameters.AddWithValue("@start_date", history.StartDate);
		//	command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
		//	reader = command.ExecuteReader();
		//	while (reader.Read())
		//	{
		//		history.History = history.History.Append(new Data.StockHistoryData(Tools.TimeConverter.dateOnlyToString(DateOnly.FromDateTime((DateTime)reader["end_date"])), Decimal.Parse("" + reader["open_price"].ToString()), Decimal.Parse("" + reader["high_price"].ToString()), Decimal.Parse("" + reader["low_price"].ToString()), Decimal.Parse("" + reader["close_price"].ToString()))).ToArray();
		//	}
		//	return history;
		//}
		//else
		//{
		//	throw new Exception();
		//}
		throw new NotImplementedException();
	}
}