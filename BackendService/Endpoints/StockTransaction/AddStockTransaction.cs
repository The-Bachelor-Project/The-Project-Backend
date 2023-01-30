using System.Data.SqlClient;

namespace BackendService;

class AddStockTransaction
{
    public static async Task<AddStockTransactionResponse> endpoint(AddStockTransactionBody body)
    {
        AddStockTransactionResponse addStockTransactionResponse = new AddStockTransactionResponse();
        if (ValidateUserToken.authenticate(body.token, body.device))
        {
            using (SqlConnection connection = Database.createConnection())
            {
                String query = "SELECT ticker,exchange FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ticker", body.ticker);
                command.Parameters.AddWithValue("@exchange", body.exchange);
                SqlDataReader reader = command.ExecuteReader();
                // TODO: Check for stock price and add back to body.date, if it does not exist
                if (!reader.Read())
                {
                    reader.Close();
                    StockInfoBody stockInfoBody = new StockInfoBody();
                    stockInfoBody.exchange = body.exchange;
                    stockInfoBody.ticker = body.ticker;
                    stockInfoBody.token = body.token;
                    await StockInfo.endpoint(stockInfoBody);
                }
                reader.Close();
                String insertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, date, price) VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @date, @price)";
                command = new SqlCommand(insertStockTransaction, connection);
                command.Parameters.AddWithValue("@portfolio", body.portfolio);
                command.Parameters.AddWithValue("@ticker", body.ticker);
                command.Parameters.AddWithValue("@exchange", body.exchange);
                command.Parameters.AddWithValue("@amount", body.amount);
                command.Parameters.AddWithValue("@amount_adjusted", body.amount_adjusted);
                command.Parameters.AddWithValue("@amount_owned", body.amount_owned);
                command.Parameters.AddWithValue("@date", body.date);
                command.Parameters.AddWithValue("@price", body.price);
                try
                {
                    command.ExecuteNonQuery();
                    addStockTransactionResponse.response = "success";
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                    addStockTransactionResponse.response = "error";
                }
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
    public String response { get; set; }
}

class AddStockTransactionBody
{
    public String portfolio { get; set; }
    public String ticker { get; set; }
    public String exchange { get; set; }
    public Double amount { get; set; }
    public Double amount_adjusted { get; set; }
    public Double amount_owned { get; set; }
    public String date { get; set; }
    public Double price { get; set; }
    public String device { get; set; }
    public String token { get; set; }
}