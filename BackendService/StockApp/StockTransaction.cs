using System.Data.SqlClient;

namespace StockApp;

public class StockTransaction
{
	public int? id { get; set; }
	public String? portfolioId { get; set; }
	public String? ticker { get; set; }
	public String? exchange { get; set; }
	public Decimal amount { get; set; }
	public Decimal? amountAdjusted { get; set; }
	public Decimal? amountOwned { get; set; }
	public int timestamp { get; set; }
	public Money? priceNative { get; set; }
	public Money? priceUSD { get; set; }

	public StockTransaction(int id, String portfolioId, String ticker, String exchange, Decimal amount, int timestamp, Money priceNative)
	{
		this.id = id;
		this.portfolioId = portfolioId;
		this.ticker = ticker;
		this.exchange = exchange;
		this.amount = amount;
		this.timestamp = timestamp;
		this.priceNative = priceNative;
	}

	public StockTransaction()
	{

	}

	/// <summary>
	/// Adds the stock transaction to the database.
	/// </summary>
	/// <returns>The added <see cref="StockTransaction"/> object.</returns>
	public async Task<StockTransaction> AddToDb()
	{
		if (amount > 0 == priceNative!.amount > 0)
		{
			priceNative!.amount *= -1;
		}

		if (ticker == null || exchange == null || portfolioId == null || priceNative!.currency == null)
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

		String getStartTracking = "SELECT start_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getStartTracking, parameters);
		if (data == null || data["start_tracking_date"] == System.DBNull.Value || Tools.TimeConverter.DateTimeToUnix((DateTime)data["start_tracking_date"]) > timestamp)
		{
			DateOnly startTrackingDate = Tools.TimeConverter.UnixTimeStampToDateOnly(timestamp);
			DateOnly endTrackingDate = DateOnly.FromDateTime(DateTime.Now);
			await new Data.Fetcher.StockFetcher().GetHistory(ticker!, exchange!, startTrackingDate, endTrackingDate, "daily", "USD");
		}

		String getSplits = "SELECT * FROM StockSplits WHERE ticker = @ticker AND exchange = @exchange AND date >= @date";
		parameters = new Dictionary<string, object>();
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		parameters.Add("@date", Tools.TimeConverter.DateOnlyToString(Tools.TimeConverter.UnixTimeStampToDateOnly(timestamp)));
		List<Dictionary<String, object>> splits = Data.Database.Reader.ReadData(getSplits, parameters);
		Decimal amountAdjustedRatio = 1;
		System.Console.WriteLine(Tools.TimeConverter.DateOnlyToString(Tools.TimeConverter.UnixTimeStampToDateOnly(timestamp)));
		int ratioOut = 1;
		int ratioIn = 1;
		foreach (Dictionary<String, object> split in splits)
		{
			ratioOut *= (int)split["ratio_out"];
			ratioIn *= (int)split["ratio_in"];
		}
		amountAdjustedRatio = (Decimal)ratioOut / (Decimal)ratioIn;


		String getAmountOwned = "SELECT TOP 1 amount_owned FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp <= @timestamp ORDER BY timestamp DESC, id DESC";
		parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", portfolioId);
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		parameters.Add("@timestamp", timestamp);
		data = Data.Database.Reader.ReadOne(getAmountOwned, parameters);

		amountOwned = 0;
		if (data != null)
		{
			amountOwned = (Decimal)data["amount_owned"];
		}
		if ((amountOwned + amountAdjusted) < 0)
		{
			throw new StatusCodeException(400, "Not enough owned stocks");
		}
		amountAdjusted = amount * amountAdjustedRatio;
		amountOwned = amountOwned + amountAdjusted;

		priceUSD = await Tools.PriceConverter.ConvertMoney(priceNative, timestamp, "USD", false);


		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String insertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, timestamp, amount_currency, currency, amount_usd) VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @timestamp, @amount_currency, @currency, @amount_usd)";
		SqlCommand command = new SqlCommand(insertStockTransaction, connection);
		command.Parameters.AddWithValue("@portfolio", portfolioId);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		command.Parameters.AddWithValue("@amount", amount);
		command.Parameters.AddWithValue("@amount_adjusted", amountAdjusted);
		command.Parameters.AddWithValue("@amount_owned", amountOwned);
		command.Parameters.AddWithValue("@timestamp", timestamp);
		command.Parameters.AddWithValue("@amount_currency", priceNative!.amount);
		command.Parameters.AddWithValue("@currency", priceNative.currency);
		command.Parameters.AddWithValue("@amount_usd", priceUSD!.amount);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not insert stock transaction into database");
		}
		String getId = "SELECT TOP 1 id FROM StockTransactions WHERE portfolio = @portfolio ORDER BY id DESC";
		Dictionary<String, object> parameters2 = new Dictionary<string, object>();
		parameters2.Add("@portfolio", portfolioId);
		Dictionary<String, object>? data2 = Data.Database.Reader.ReadOne(getId, parameters2);
		if (data2 == null)
		{
			throw new StatusCodeException(500, "Could not get id of inserted stock transaction");
		}
		id = (int)data2["id"];
		return this;
	}

	/// <summary>
	/// Deletes the stock transaction from the database.
	/// </summary>
	/// <returns>Void, but in task.</returns>
	public Task Delete()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String checkIfTransactionExistsQuery = "SELECT * FROM StockTransactions WHERE id = @id";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", id!);
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