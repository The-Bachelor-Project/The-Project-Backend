using System.Data.SqlClient;

namespace BackendService;

class StockHistory  //TODO: this is not done at all
{
	public static async Task<StockHistoryResponse> endpoint(StockHistoryBody body)
	{
		StockHistoryResponse stockHistoryResponse = new StockHistoryResponse("error");
		using (SqlConnection connection = Database.createConnection())
		{
			System.Console.WriteLine("History");
			String getTrackingDateQuery = "SELECT start_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
			SqlCommand command = new SqlCommand(getTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", body.ticker);
			command.Parameters.AddWithValue("@exchange", body.exchange);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				DateOnly trackingDate;
				try
				{
					trackingDate = DateOnly.Parse("" + reader["start_tracking_date"].ToString());
				}
				catch (Exception)
				{
					trackingDate = DateOnly.FromDateTime(DateTime.Now);
				}

				reader.Close();

				DateOnly startDate = DateOnly.Parse(body.start_date);
				if (startDate < trackingDate)
				{
					System.Console.WriteLine("time1: " + TimeConverter.dateOnlyToString(startDate));
					System.Console.WriteLine("time1: " + TimeConverter.dateOnlyToString(trackingDate.AddDays(-1)));
					await DataFetcher.stockHistory(body.ticker, body.exchange, startDate, trackingDate.AddDays(-1));

					String updateStockTrackingDatesQuery = "UPDATE Stocks SET start_tracking_date = @start_tracking_date, end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
					command = new SqlCommand(updateStockTrackingDatesQuery, connection);
					command.Parameters.AddWithValue("@start_tracking_date", TimeConverter.dateOnlyToString(startDate));
					command.Parameters.AddWithValue("@end_tracking_date", TimeConverter.dateOnlyToString(trackingDate.AddDays(-1)));
					command.Parameters.AddWithValue("@ticker", body.ticker);
					command.Parameters.AddWithValue("@exchange", body.exchange);
					command.ExecuteNonQuery();
				}
			}
		}

		return stockHistoryResponse;
	}
}

class StockHistoryResponse
{
	public StockHistoryResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class StockHistoryBody
{
	public StockHistoryBody(string ticker, string exchange, String start_date)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.start_date = start_date;
	}

	public String ticker { get; set; }
	public String exchange { get; set; }
	public String start_date { get; set; }
}