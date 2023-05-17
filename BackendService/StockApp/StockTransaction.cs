using System.Data.SqlClient;

namespace StockApp;

public class StockTransaction
{
	public int? id { get; set; }
	public String? portfolioId { get; set; }
	public String? ticker { get; set; }
	public String? exchange { get; set; }
	public Decimal? amount { get; set; }
	public Decimal? amountAdjusted { get; set; }
	public Decimal? amountOwned { get; set; }
	public int? timestamp { get; set; }
	public Money? price { get; set; }



	public async Task<StockTransaction> AddToDb() //FIXME at some point also ORDER by index as well as timestamp to avoid some issues with calculating amount_owned 
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();

		try
		{
			await new Data.Fetcher.StockFetcher().GetProfile(ticker!, exchange!);
		}
		catch (Exception)
		{
			throw new CouldNotGetStockException();
		}

		String getAmountOwned = "SELECT TOP 1 amount_owned FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp <= @timestamp ORDER BY timestamp,id DESC";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", portfolioId);
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		parameters.Add("@timestamp", timestamp);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getAmountOwned, parameters);

		decimal amountOwned = 0;
		decimal amountAdjusted = amount!.Value;
		if (data != null)
		{
			amountOwned = Convert.ToDecimal(data["amount_owned"]);
		}

		String insertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, timestamp, price_amount, price_currency) OUTPUT INSERTED.id VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @timestamp, @price_amount, @price_currency)";
		SqlCommand command = new SqlCommand(insertStockTransaction, connection);
		command.Parameters.AddWithValue("@portfolio", portfolioId);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		command.Parameters.AddWithValue("@amount", amount);
		command.Parameters.AddWithValue("@amount_adjusted", amountAdjusted);
		command.Parameters.AddWithValue("@amount_owned", amountOwned + amountAdjusted);
		command.Parameters.AddWithValue("@timestamp", timestamp);
		command.Parameters.AddWithValue("@price_amount", price!.amount);
		command.Parameters.AddWithValue("@price_currency", price.currency);
		id = int.Parse((command.ExecuteScalar()).ToString()!);



		String updateStockTransactions = "UPDATE StockTransactions SET amount_owned = amount_owned + @amount_adjusted WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp > @timestamp OR (timestamp = @timestamp AND id > @id)";
		SqlCommand command2 = new SqlCommand(updateStockTransactions, connection);
		command2.Parameters.AddWithValue("@portfolio", portfolioId);
		command2.Parameters.AddWithValue("@id", id);
		command2.Parameters.AddWithValue("@ticker", ticker);
		command2.Parameters.AddWithValue("@exchange", exchange);
		command2.Parameters.AddWithValue("@amount_adjusted", amountAdjusted);
		command2.Parameters.AddWithValue("@timestamp", timestamp);
		command2.ExecuteNonQuery();



		return this;
	}

	public Portfolio GetPortfolio()
	{
		return new Portfolio(portfolioId!);
	}

	public Task DeleteFromDb()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String deleteQuery = "DELETE FROM StockTransactions WHERE id = @id";
		SqlCommand command = new SqlCommand(deleteQuery, connection);
		command.Parameters.AddWithValue("@id", id);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception)
		{
			throw new Exception();
		}
		return Task.CompletedTask;
	}
}