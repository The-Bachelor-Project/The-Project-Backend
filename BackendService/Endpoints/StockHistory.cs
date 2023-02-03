using System.Data.SqlClient;

namespace BackendService;

class StockHistory  //TODO: this is not done at all
{
	public static async Task<StockHistoryResponse> endpoint(StockHistoryBody body)
	{
		StockHistoryResponse stockHistoryResponse = new StockHistoryResponse("error");
		using (SqlConnection connection = Database.createConnection())
		{
			String getTrackingDateQuery = "SELECT tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
			SqlCommand command = new SqlCommand(getTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", body.ticker);
			command.Parameters.AddWithValue("@exchange", body.exchange);
			SqlDataReader reader = command.ExecuteReader();
			if (!reader.Read())
			{
				int trackingDate = Int32.Parse(reader["tracking_date"].ToString());
			}
			try
			{
				command.ExecuteNonQuery();
				stockHistoryResponse.response = "success";
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				stockHistoryResponse.response = "error";
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
	public StockHistoryBody(string ticker, string exchange, int start_time)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.start_time = start_time;
	}

	public String ticker { get; set; }
	public String exchange { get; set; }
	public int start_time { get; set; }
}