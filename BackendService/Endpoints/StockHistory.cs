using System.Data.SqlClient;

namespace BackendService;

class StockHistory
{
	public static async Task<StockHistoryResponse> endpoint(StockHistoryBody body)
	{
		StockHistoryResponse stockHistoryResponse = new StockHistoryResponse("error");
		using (SqlConnection connection = Database.createConnection())
		{
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
					trackingDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]);
				}
				catch (Exception)
				{
					trackingDate = DateOnly.FromDateTime(DateTime.Now);
				}

				reader.Close();

				DateOnly startDate = DateOnly.Parse(body.start_date);
				if (startDate < trackingDate)
				{
					await StockPricesUpdater.update(body.ticker, body.exchange, startDate);
				}


				DateOnly endDate = body.end_date == "" ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.Parse(body.end_date);

				String getStockHistoryQuery = "SELECT * FROM StockPrices WHERE ticker = @ticker AND exchange = @exchange AND date >= @start_date";
				command = new SqlCommand(getStockHistoryQuery, connection);
				command.Parameters.AddWithValue("@ticker", body.ticker);
				command.Parameters.AddWithValue("@exchange", body.exchange);
				command.Parameters.AddWithValue("@start_date", body.start_date);
				reader = command.ExecuteReader();
				while (reader.Read())
				{
					stockHistoryResponse.history = stockHistoryResponse.history.Append(new StockHistoryInfo(TimeConverter.dateOnlyToString(DateOnly.FromDateTime((DateTime)reader["date"])), Decimal.Parse("" + reader["open_price"].ToString()), Decimal.Parse("" + reader["high_price"].ToString()), Decimal.Parse("" + reader["low_price"].ToString()), Decimal.Parse("" + reader["close_price"].ToString()), int.Parse("" + reader["volume"].ToString()))).ToArray();
				}
				stockHistoryResponse.response = "success";
			}
			else
			{
				throw new Exception();
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
		history = new StockHistoryInfo[] { };
	}

	public String response { get; set; }
	public StockHistoryInfo[] history { get; set; }
}

class StockHistoryBody
{
	public StockHistoryBody(string ticker, string exchange, String start_date, String end_date, String interval)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.start_date = start_date;
		this.end_date = end_date;
		this.interval = interval;
	}

	public String ticker { get; set; }
	public String exchange { get; set; }
	public String start_date { get; set; }
	public String end_date { get; set; }
	public String interval { get; set; }
}

class StockHistoryInfo
{
	public StockHistoryInfo(string date, decimal open_price, decimal high_price, decimal low_price, decimal close_price, int volume)
	{
		this.date = date;
		this.open_price = open_price;
		this.high_price = high_price;
		this.low_price = low_price;
		this.close_price = close_price;
		this.volume = volume;
	}

	public String date { get; set; }
	public Decimal open_price { get; set; }
	public Decimal high_price { get; set; }
	public Decimal low_price { get; set; }
	public Decimal close_price { get; set; }
	public int volume { get; set; }
}