using System.Data.SqlClient;

namespace StockApp;

public class StockTransaction
{
	public String? id { get; set; }
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
		SqlConnection connection = new Data.Database.Connection().Create();

		try
		{
			await new Data.Fetcher.StockFetcher().GetProfile(ticker!, exchange!);
		}
		catch (Exception)
		{
			throw new CouldNotGetStockException();
		}

		String getAmountOwned = "SELECT TOP 1 amount_owned FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp < @timestamp ORDER BY timestamp DESC";

		SqlCommand command3 = new SqlCommand(getAmountOwned, connection);
		command3.Parameters.AddWithValue("@portfolio", portfolioId);
		command3.Parameters.AddWithValue("@ticker", ticker);
		command3.Parameters.AddWithValue("@exchange", exchange);
		command3.Parameters.AddWithValue("@timestamp", timestamp);

		decimal amountOwned = 0;
		decimal amountAdjusted = 0;

		using (SqlDataReader reader = command3.ExecuteReader())
		{
			amountOwned = reader.Read() ? reader.GetDecimal(0) : 0;
			amountAdjusted = amount!.Value; //TODO: Should be adjusted in the future
			reader.Close();
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
		id = (command.ExecuteScalar()).ToString();



		String updateStockTransactions = "UPDATE StockTransactions SET amount_owned = amount_owned + @amount_adjusted WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp > @timestamp";
		SqlCommand command2 = new SqlCommand(updateStockTransactions, connection);
		command2.Parameters.AddWithValue("@portfolio", portfolioId);
		command2.Parameters.AddWithValue("@ticker", ticker);
		command2.Parameters.AddWithValue("@exchange", exchange);
		command2.Parameters.AddWithValue("@amount_adjusted", amountAdjusted);
		command2.Parameters.AddWithValue("@timestamp", timestamp);
		command2.ExecuteNonQuery();



		return this; //TODO Update Id before returning, maybe by making a function on the database
	}

	public Portfolio GetPortfolio()
	{
		return new Portfolio(portfolioId!);
	}
}