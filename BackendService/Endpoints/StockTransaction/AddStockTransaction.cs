using System.Data.SqlClient;

namespace BackendService;

class AddStockTransaction
{
	public static async Task<AddStockTransactionResponse> endpoint(AddStockTransactionBody body)
	{
		AddStockTransactionResponse addStockTransactionResponse = new AddStockTransactionResponse("error");
		if (ValidateUserToken.authenticate(body.token))
		{
			using (SqlConnection connection = Database.createConnection())
			{

				try
				{
					await StockInfo.getStock(body.ticker, body.exchange);

					String insertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, timestamp, price) VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @timestamp, @price)";

					SqlCommand command = new SqlCommand(insertStockTransaction, connection);
					command.Parameters.AddWithValue("@portfolio", body.portfolio);
					command.Parameters.AddWithValue("@ticker", body.ticker);
					command.Parameters.AddWithValue("@exchange", body.exchange);
					command.Parameters.AddWithValue("@amount", body.amount);
					command.Parameters.AddWithValue("@amount_adjusted", body.amount); //TODO: Should be adjusted in the future
					command.Parameters.AddWithValue("@amount_owned", body.amount); //TODO: Should be calculated in the future
					command.Parameters.AddWithValue("@timestamp", body.timestamp);
					command.Parameters.AddWithValue("@price", body.price);
					try
					{
						command.ExecuteNonQuery();
						addStockTransactionResponse.response = "success";
					}
					catch (System.Exception)
					{
						addStockTransactionResponse.response = "error";
					}
				}
				catch (Exception) { }

			}
			return addStockTransactionResponse;
		}
		else
		{
			addStockTransactionResponse.response = "Authentication problem";
			return addStockTransactionResponse;
		}
	}
}

class AddStockTransactionResponse
{
	public AddStockTransactionResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class AddStockTransactionBody
{
	public AddStockTransactionBody(string portfolio, string ticker, string exchange, decimal amount, int timestamp, decimal price, string token)
	{
		this.portfolio = portfolio;
		this.ticker = ticker;
		this.exchange = exchange;
		this.amount = amount;
		this.timestamp = timestamp;
		this.price = price;
		this.token = token;
	}

	public String portfolio { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
	public Decimal amount { get; set; }
	public int timestamp { get; set; }
	public Decimal price { get; set; }
	public String token { get; set; }
}