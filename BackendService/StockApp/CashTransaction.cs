namespace StockApp;

using System.Data.SqlClient;

public class CashTransaction
{
	public int? id { get; set; }
	public String? portfolioId { get; set; }
	public Money? nativeAmount { get; set; }
	public Money? usdAmount { get; set; }
	public int? timestamp { get; set; }
	public String? type { get; set; }
	public String? description { get; set; }

	public CashTransaction(String portfolioId, Money nativeAmount, int timestamp, String type, String? description)
	{
		this.portfolioId = portfolioId;
		this.nativeAmount = nativeAmount;
		this.timestamp = timestamp;
		this.type = type;
		this.description = description;
	}

	public CashTransaction()
	{
	}

	public async Task<CashTransaction> AddToDb()
	{
		if (portfolioId == null || nativeAmount == null || nativeAmount.currency == null || timestamp == null || type == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}

		if (!(Tools.ValidCurrency.Check(nativeAmount.currency)))
		{
			throw new StatusCodeException(400, "Invalid currency: " + nativeAmount.currency);
		}

		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String getBalance = "SELECT TOP 1 balance FROM CashTransactions WHERE portfolio = @portfolio AND timestamp <= @timestamp ORDER BY id DESC, timestamp DESC";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", portfolioId);
		parameters.Add("@currency", nativeAmount.currency);
		parameters.Add("@timestamp", timestamp);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getBalance, parameters);

		usdAmount = await Tools.PriceConverter.ConvertMoney(nativeAmount, timestamp, "USD", false);

		String insertCashTransactionQuery = "INSERT INTO CashTransactions (portfolio, currency, native_amount, amount, timestamp, type, description) OUTPUT INSERTED.id VALUES (@portfolio, @currency, @native_amount, @amount, @timestamp, @type, @description)";
		SqlCommand command = new SqlCommand(insertCashTransactionQuery, connection);
		command.Parameters.AddWithValue("@portfolio", portfolioId);
		command.Parameters.AddWithValue("@currency", nativeAmount.currency);
		command.Parameters.AddWithValue("@native_amount", nativeAmount.amount);
		command.Parameters.AddWithValue("@amount", usdAmount.amount);
		command.Parameters.AddWithValue("@timestamp", timestamp);
		command.Parameters.AddWithValue("@type", type);
		command.Parameters.AddWithValue("@description", description);
		try
		{
			id = int.Parse((command.ExecuteScalar()).ToString()!);
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not add cash transaction to database");
		}

		String updateCashTransactions = "UPDATE CashTransactions SET balance = balance + @amount WHERE portfolio = @portfolio AND timestamp > @timestamp OR (timestamp = @timestamp AND id > @id)";
		command = new SqlCommand(updateCashTransactions, connection);
		command.Parameters.AddWithValue("@portfolio", portfolioId);
		command.Parameters.AddWithValue("@currency", nativeAmount.currency);
		command.Parameters.AddWithValue("@timestamp", timestamp);
		command.Parameters.AddWithValue("@id", id);
		command.Parameters.AddWithValue("@amount", usdAmount.amount);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not update cash transactions");
		}
		return this;
	}

	public void DeleteFromDb()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}

		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String checkIfTransactionExistsQuery = "SELECT id FROM CashTransactions WHERE id = @id";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(checkIfTransactionExistsQuery, parameters);
		if (data == null)
		{
			throw new StatusCodeException(404, "Cash transaction not found");
		}
		String deleteTransactionQuery = "DELETE FROM CashTransactions WHERE id = @id";
		SqlCommand command = new SqlCommand(deleteTransactionQuery, connection);
		command.Parameters.AddWithValue("@id", id);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not delete cash transaction");
		}
	}
}
