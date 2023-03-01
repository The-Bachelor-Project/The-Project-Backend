using System.Data.SqlClient;

namespace DatabaseService;

class StockTransaction
{
	public static async Task<String> Add(Data.StockTransaction transaction)
	{
		SqlConnection Connection = Database.createConnection();

		try
		{
			await DatabaseService.StockProfile.Get(transaction.ticker, transaction.exchange);
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new CouldNotGetStockException();
		}

		String InsertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, timestamp, price_amount, price_currency) VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @timestamp, @price_amount, @price_currency)";

		SqlCommand Command = new SqlCommand(InsertStockTransaction, Connection);
		Command.Parameters.AddWithValue("@portfolio", transaction.portfolio);
		Command.Parameters.AddWithValue("@ticker", transaction.ticker);
		Command.Parameters.AddWithValue("@exchange", transaction.exchange);
		Command.Parameters.AddWithValue("@amount", transaction.amount);
		Command.Parameters.AddWithValue("@amount_adjusted", transaction.amount); //TODO: Should be adjusted in the future
		Command.Parameters.AddWithValue("@amount_owned", transaction.amount); //TODO: Should be calculated in the future
		Command.Parameters.AddWithValue("@timestamp", transaction.timestamp);
		Command.Parameters.AddWithValue("@price_amount", transaction.price.Amount);
		Command.Parameters.AddWithValue("@price_currency", transaction.price.Currency.ToString());
		Command.ExecuteNonQuery();

		return "";
	}
}