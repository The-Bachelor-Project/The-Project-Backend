namespace StockApp;

using System.Data.SqlClient;

public class CashTransaction
{
	public int? id { get; set; }
	public String? portfolioId { get; set; }
	public Money? nativeAmount { get; set; }
	public Money? usdAmount { get; set; }
	public int? timestamp { get; set; }
	public String? description { get; set; }

	public CashTransaction(String portfolioId, Money nativeAmount, int timestamp, String? description)
	{
		this.portfolioId = portfolioId;
		this.nativeAmount = nativeAmount;
		this.timestamp = timestamp;
		this.description = description;
	}

	public CashTransaction()
	{
	}

	public async Task<CashTransaction> AddToDb()
	{
		if (portfolioId == null || nativeAmount == null || nativeAmount.currency == null || timestamp == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}

		if (!(Tools.ValidCurrency.Check(nativeAmount.currency)))
		{
			throw new StatusCodeException(400, "Invalid currency: " + nativeAmount.currency);
		}
		usdAmount = await Tools.PriceConverter.ConvertMoney(nativeAmount, (int)timestamp, "USD", false);

		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String insertCashTransactionQuery = "INSERT INTO CashTransactions (portfolio, currency, amount_currency, amount_usd, timestamp, description) VALUES (@portfolio, @currency, @amount_currency, @amount_usd, @timestamp, @description)";
		SqlCommand command = new SqlCommand(insertCashTransactionQuery, connection);
		command.Parameters.AddWithValue("@portfolio", portfolioId);
		command.Parameters.AddWithValue("@currency", nativeAmount.currency);
		command.Parameters.AddWithValue("@amount_currency", nativeAmount.amount);
		command.Parameters.AddWithValue("@amount_usd", usdAmount.amount);
		command.Parameters.AddWithValue("@timestamp", timestamp);
		command.Parameters.AddWithValue("@description", description);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not add cash transaction to database");
		}
		String getLastInsertIdQuery = "SELECT TOP 1 id FROM CashTransactions WHERE @portfolio = portfolio ORDER BY id DESC";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", portfolioId);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getLastInsertIdQuery, parameters);
		if (data == null)
		{
			throw new StatusCodeException(500, "Could not get last insert id");
		}
		id = (int)data["id"];
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
