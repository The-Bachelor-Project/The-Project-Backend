using System.Data.SqlClient;

namespace BackendService;


class StockInfo
{
	public String? ticker { get; set; }
	public String? exchange { get; set; }
	public String? name { get; set; }
	public String? industry { get; set; }
	public String? sector { get; set; }
	public String? website { get; set; }
	public String? country { get; set; }
	int trackingDate;


	public static async Task<StockInfoResponse> endpoint(StockInfoBody body)
	{
		StockInfoResponse stockResponse = new StockInfoResponse("success", await getStock(body.ticker, body.exchange));
		try
		{
			stockResponse.stock = await getStock(body.ticker, body.exchange);
			stockResponse.response = "success";
		}
		catch (Exception e)
		{
			stockResponse.response = "error";
		}
		return stockResponse;
	}

	public static async Task<StockInfo> getStock(String ticker, String exchange)
	{
		StockInfo result = new StockInfo();
		result.ticker = ticker;
		result.exchange = exchange;

		using (SqlConnection connection = Database.createConnection())
		{
			String query = "SELECT * FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@ticker", ticker);
			command.Parameters.AddWithValue("@exchange", exchange);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				result.name = reader["company_name"].ToString();
				result.industry = reader["industry"].ToString();
				result.sector = reader["sector"].ToString();
				result.website = reader["website"].ToString();
				result.country = reader["country"].ToString();
			}
			else
			{
				result = await DataFetcher.stock(ticker, exchange);
				_saveStock(result);
			}
		}

		return result;
	}

	private static void _saveStock(StockInfo newStock)
	{
		String tags = StockTagGenerator.generate(newStock);
		using (SqlConnection connection = Database.createConnection())
		{
			String query = "INSERT INTO Stocks (ticker, exchange, company_name, industry, sector, website, country, tags) VALUES (@ticker, @exchange, @name, @industry, @sector, @website, @country, @tags)";
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@ticker", newStock.ticker);
			command.Parameters.AddWithValue("@exchange", newStock.exchange);
			command.Parameters.AddWithValue("@name", newStock.name);
			command.Parameters.AddWithValue("@industry", newStock.industry);
			command.Parameters.AddWithValue("@sector", newStock.sector);
			command.Parameters.AddWithValue("@website", newStock.website);
			command.Parameters.AddWithValue("@country", newStock.country);
			command.Parameters.AddWithValue("@tags", tags);
			command.ExecuteNonQuery();
		}
	}
}


class StockInfoResponse
{
	public StockInfoResponse(String response, StockInfo stock)
	{
		this.response = response;
		this.stock = stock;
	}
	public StockInfoResponse(String response)
	{
		this.response = response;
	}

	public String response { get; set; }
	public StockInfo? stock { get; set; }
}

class StockInfoBody
{
	public StockInfoBody(String token, String ticker, String exchange)
	{
		this.token = token;
		this.ticker = ticker;
		this.exchange = exchange;
	}
	public String token { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
}