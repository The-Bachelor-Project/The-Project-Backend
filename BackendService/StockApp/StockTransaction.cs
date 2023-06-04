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
	public Money? priceNative { get; set; }
	public Money? priceUSD { get; set; }

	public StockTransaction(int id, String portfolioId, String ticker, String exchange, Decimal amount, int timestamp, Money price)
	{
		this.id = id;
		this.portfolioId = portfolioId;
		this.ticker = ticker;
		this.exchange = exchange;
		this.amount = amount;
		this.timestamp = timestamp;
		this.priceNative = price;
	}

	public StockTransaction()
	{
	}



	public async Task<StockTransaction> AddToDb() //FIXME at some point also ORDER by index as well as timestamp to avoid some issues with calculating amount_owned 
	{

		if (ticker == null || exchange == null || amount == null || timestamp == null || portfolioId == null || priceNative!.currency == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}

		try
		{
			await new Data.Fetcher.StockFetcher().GetProfile(ticker!, exchange!);
		}
		catch (Exception)
		{
			throw new StatusCodeException(404, "Could not find stock " + exchange + ":" + ticker);
		}
		if (!(Tools.ValidCurrency.Check(priceNative!.currency)))
		{
			throw new StatusCodeException(400, "Invalid currency " + priceNative!.currency);
		}

		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String getAmountOwned = "SELECT TOP 1 amount_owned,balance FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp <= @timestamp ORDER BY timestamp DESC, id DESC";
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
			amountOwned = (Decimal)data["amount_owned"];
		}
		if (amountOwned + amountAdjusted < 0)
		{
			throw new StatusCodeException(400, "Not enough owned stocks");
		}

		priceUSD = await Tools.PriceConverter.ConvertMoney(priceNative, timestamp, "USD", false);

		String insertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, timestamp, amount_currency, currency, amount_usd) OUTPUT INSERTED.id VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @timestamp, @amount_currency, @currency, @amount_usd)";
		SqlCommand command = new SqlCommand(insertStockTransaction, connection);
		command.Parameters.AddWithValue("@portfolio", portfolioId);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		command.Parameters.AddWithValue("@amount", amount);
		command.Parameters.AddWithValue("@amount_adjusted", amountAdjusted);
		command.Parameters.AddWithValue("@amount_owned", amountOwned + amountAdjusted);
		command.Parameters.AddWithValue("@timestamp", timestamp);
		command.Parameters.AddWithValue("@amount_currency", priceNative!.amount);
		command.Parameters.AddWithValue("@currency", priceNative.currency);
		command.Parameters.AddWithValue("@amount_usd", priceUSD!.amount);
		try
		{
			id = int.Parse((command.ExecuteScalar()).ToString()!);
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not insert stock transaction into database");
		}

		String updateStockTransactions = "UPDATE StockTransactions SET amount_owned = amount_owned + @amount_adjusted WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp > @timestamp OR (timestamp = @timestamp AND id > @id)";
		SqlCommand command2 = new SqlCommand(updateStockTransactions, connection);
		command2.Parameters.AddWithValue("@portfolio", portfolioId);
		command2.Parameters.AddWithValue("@id", id);
		command2.Parameters.AddWithValue("@ticker", ticker);
		command2.Parameters.AddWithValue("@exchange", exchange);
		command2.Parameters.AddWithValue("@amount_adjusted", amountAdjusted);
		command2.Parameters.AddWithValue("@timestamp", timestamp);
		try
		{
			command2.ExecuteNonQuery();
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(409, "Could not update stock transactions in database");
		}
		return this;
	}

	public Portfolio GetPortfolio()
	{
		if (portfolioId == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String checkIfPortfolioExistsQuery = "SELECT * FROM Portfolios WHERE uid = @id";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", portfolioId);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(checkIfPortfolioExistsQuery, parameters);
		if (data == null)
		{
			throw new StatusCodeException(404, "Could not find portfolio with id " + portfolioId);
		}
		return new Portfolio(portfolioId!);
	}

	public Task DeleteFromDb()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String checkIfTransactionExistsQuery = "SELECT * FROM StockTransactions WHERE id = @id";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(checkIfTransactionExistsQuery, parameters);
		if (data == null)
		{
			throw new StatusCodeException(404, "Could not find stock transaction with id " + id);
		}

		String deleteQuery = "DELETE FROM StockTransactions WHERE id = @id";
		SqlCommand command = new SqlCommand(deleteQuery, connection);
		command.Parameters.AddWithValue("@id", id);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not delete stock transaction from database with id " + id);
		}
		return Task.CompletedTask;
	}
}