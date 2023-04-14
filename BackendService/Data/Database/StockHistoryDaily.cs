using System.Data.SqlClient;
using Data.Interfaces;

namespace Data.Database;

public class StockHistoryDaily : IStockHistoryDaily
{
	public StockHistoryDaily()
	{
	}

	public async Task<StockHistory> Usd(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String getStockHistoryQuery = "SELECT * FROM GetStockPrices(@ticker, @exchange, 'daily', @start_date, @end_date)";
		SqlCommand command = new SqlCommand(getStockHistoryQuery, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		command.Parameters.AddWithValue("@start_date", Tools.TimeConverter.dateOnlyToString(startDate));
		command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
		SqlDataReader reader = command.ExecuteReader();

		StockHistory Result = new StockHistory(ticker, exchange, "daily");
		while (reader.Read())
		{
			Result.History = Result.History.Append(new Data.StockHistoryData(
				DateOnly.FromDateTime((DateTime)reader["end_date"]),
				Decimal.Parse("" + reader["open_price"].ToString()),
				Decimal.Parse("" + reader["high_price"].ToString()),
				Decimal.Parse("" + reader["low_price"].ToString()),
				Decimal.Parse("" + reader["close_price"].ToString())
			)).ToArray();
		}


		Result.StartDate = Result.History.First().Date;
		Result.EndDate = Result.History.Last().Date;


		return Result;
	}
}