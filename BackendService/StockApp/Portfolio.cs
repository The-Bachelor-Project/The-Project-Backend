using System.Data.SqlClient;

namespace StockApp;

public class Portfolio
{

	public Portfolio(string id)
	{
		this.id = id;
	}
	public Portfolio(string name, string owner, string currency, bool trackBalance)
	{
		this.name = name;
		this.owner = owner;
		this.currency = currency;
		this.trackBalance = trackBalance;
	}
	public Portfolio(string id, string name, string owner, string currency, bool trackBalance)
	{
		this.id = id;
		this.name = name;
		this.owner = owner;
		this.currency = currency;
		this.trackBalance = trackBalance;
	}

	public Portfolio(String name, String currency, List<Data.DatePriceOHLC> valueHistory, List<Data.Position> positions, List<Data.Dividend> dividendHistory, List<Data.CashBalance> cashBalance)
	{
		this.name = name;
		this.currency = currency;
		this.valueHistory = valueHistory;
		this.positionHistories = positions;
		this.dividendHistory = dividendHistory;
		this.cashBalance = cashBalance;
	}

	public String? id { get; set; }
	public String? name { get; set; }
	public String? owner { get; set; }
	public String? currency { get; set; }
	public Boolean? trackBalance { get; set; }

	public List<Data.DatePriceOHLC>? valueHistory { get; set; }
	public List<StockTransaction> stockTransactions { get; set; } = new List<StockTransaction>();
	public List<CashTransaction> cashTransactions { get; set; } = new List<CashTransaction>();
	public List<StockPosition> stockPositions { get; set; } = new List<StockPosition>();
	public List<Data.Position>? positionHistories { get; set; }
	public List<Data.Dividend>? dividendHistory { get; set; }
	public List<Data.CashBalance> cashBalance { get; set; } = new List<Data.CashBalance>();


	public Portfolio AddToDb()
	{
		if (name == null || owner == null || currency == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		if (!(Tools.ValidCurrency.Check(currency!)))
		{
			throw new StatusCodeException(400, "Currency " + currency + " is not supported");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		id = Tools.RandomString.Generate(32);
		String query = "INSERT INTO Portfolios (uid, name, owner, currency) VALUES (@uid, @name, @owner, @currency)";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@uid", id);
		command.Parameters.AddWithValue("@name", name);
		command.Parameters.AddWithValue("@owner", owner);
		command.Parameters.AddWithValue("@currency", currency);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to create portfolio");
		}


		return this;
	}

	public User GetOwner()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		String query = "SELECT owner FROM Portfolios WHERE uid = @uid";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@uid", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);

		if (data != null)
		{
			String userId = data["owner"].ToString()!;
			User user = new User(userId);
			return user;
		}
		throw new StatusCodeException(404, "No portfolio with uid " + id + " was found");

	}


	public Portfolio UpdateStockTransactions()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		String query = "SELECT * FROM StockTransactions WHERE portfolio = @portfolio";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", id);
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(query, parameters);

		stockTransactions = new List<StockTransaction>();
		foreach (Dictionary<String, object> row in data)
		{
			stockTransactions.Add(new StockTransaction());
			stockTransactions.Last().id = int.Parse(row["id"].ToString()!);
			stockTransactions.Last().portfolioId = id;
			stockTransactions.Last().ticker = row["ticker"].ToString();
			stockTransactions.Last().exchange = row["exchange"].ToString();
			stockTransactions.Last().amount = Convert.ToDecimal(row["amount"]);
			stockTransactions.Last().amountAdjusted = Convert.ToDecimal(row["amount_adjusted"]);
			stockTransactions.Last().amountOwned = Convert.ToDecimal(row["amount_owned"]);
			stockTransactions.Last().timestamp = Convert.ToInt32(row["timestamp"]);
			stockTransactions.Last().priceNative = new Money(Convert.ToDecimal(row["amount_currency"]), row["currency"].ToString()!);
		}
		return this;
	}

	public Portfolio UpdateStockPositions()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		String query = "SELECT DISTINCT ticker, exchange FROM StockTransactions WHERE portfolio = @portfolio";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", id);
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(query, parameters);

		stockPositions = new List<StockPosition>();
		foreach (Dictionary<String, object> row in data)
		{
			stockPositions.Add(new StockPosition(this, new Stock(row["ticker"].ToString()!, row["exchange"].ToString()!)));
			System.Console.WriteLine(stockPositions.Last().stock.ticker + " " + stockPositions.Last().stock.exchange);
		}
		return this;
	}

	public async Task<Portfolio> GetValueHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		if (id == null || currency == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		if (!(Tools.ValidCurrency.Check(currency)))
		{
			throw new StatusCodeException(400, "Currency" + currency + " is not supported");
		}
		if (startDate > endDate)
		{
			throw new StatusCodeException(400, "Start date must be before end date");
		}

		UpdateStockPositions();
		UpdateStockTransactions();

		valueHistory = new List<Data.DatePriceOHLC>();
		positionHistories = new List<Data.Position>();
		dividendHistory = new List<Data.Dividend>();

		foreach (StockPosition position in stockPositions)
		{
			Data.Position dataPosition = await position.GetValueHistory(currency, startDate, endDate);
			if (dataPosition.valueHistory.Count > 0)
			{
				positionHistories.Add(dataPosition);
				valueHistory = Data.DatePriceOHLC.AddLists(valueHistory, dataPosition.valueHistory);
				dividendHistory.AddRange(dataPosition.dividends);
			}
		}
		List<Data.Transaction> allTransactions = await GetOwner().GetTransactions(currency);
		int firstIndex = allTransactions.FindLastIndex(x => x.timestamp <= Tools.TimeConverter.DateOnlyToUnix(startDate) && x.portfolio == id);
		int lastIndex = allTransactions.FindIndex(x => x.timestamp <= Tools.TimeConverter.DateOnlyToUnix(endDate) && x.portfolio == id);
		if (firstIndex == -1)
		{
			firstIndex = 0;
		}
		List<Data.Transaction> newTransactions = new List<Data.Transaction>();
		if (lastIndex != -1)
		{
			newTransactions = allTransactions.GetRange(firstIndex < 0 ? 0 : firstIndex, ((lastIndex - firstIndex + 1) < 0 ? 0 : (lastIndex - firstIndex + 1)));
		}
		User user = GetOwner();
		cashBalance = user.InsertMissingValues(cashBalance);
		return new Portfolio(name!, currency, valueHistory, positionHistories, dividendHistory, cashBalance);
	}

	public Portfolio ChangeName(string newName)
	{
		if (id == null || newName == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "UPDATE Portfolios SET name = @name WHERE uid = @uid";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@name", newName);
		command.Parameters.AddWithValue("@uid", id);
		try
		{
			command.ExecuteNonQuery();
			this.name = newName;
			return this;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(409, "Could not change name of portfolio with id " + id);
		}
	}

	public Portfolio ChangeCurrency(String newCurrency)
	{
		if (id == null || newCurrency == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		if (!(Tools.ValidCurrency.Check(newCurrency)))
		{
			throw new StatusCodeException(400, "Currency" + currency + " is not supported");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();

		String updateCurrency = "UPDATE Portfolios SET currency = @currency WHERE uid = @uid";
		SqlCommand command = new SqlCommand(updateCurrency, connection);
		command.Parameters.AddWithValue("@currency", newCurrency);
		command.Parameters.AddWithValue("@uid", id);
		try
		{
			command.ExecuteNonQuery();
			this.currency = newCurrency;
			return this;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(409, "Could not change currency of portfolio with id " + id);
		}
	}

	public StockTransaction GetStockTransaction(int transactionID)
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		System.Console.WriteLine("ID: " + transactionID);
		String query = "SELECT * FROM StockTransactions WHERE id = @id AND portfolio = @portfolio";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", transactionID);
		parameters.Add("@portfolio", id!);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);

		if (data != null)
		{
			StockTransaction stockTransaction = new StockTransaction();
			stockTransaction.id = int.Parse(data["id"].ToString()!);
			stockTransaction.portfolioId = data["portfolio"].ToString();
			stockTransaction.ticker = data["ticker"].ToString();
			stockTransaction.exchange = data["exchange"].ToString();
			stockTransaction.amount = Convert.ToDecimal(data["amount"]);
			stockTransaction.amountAdjusted = Convert.ToDecimal(data["amount_adjusted"]);
			stockTransaction.amountOwned = Convert.ToDecimal(data["amount_owned"]);
			stockTransaction.timestamp = Convert.ToInt32(data["timestamp"]);
			stockTransaction.priceNative = new Money(Convert.ToDecimal(data["amount_currency"]), data["currency"].ToString()!);
			return stockTransaction;
		}
		throw new StatusCodeException(404, "Could not find stock transaction with id " + transactionID);
	}

	public void Delete()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		String queryCheck = "SELECT * FROM Portfolios WHERE uid = @uid";
		Dictionary<String, object> parametersCheck = new Dictionary<string, object>();
		parametersCheck.Add("@uid", id!);
		Dictionary<String, object>? dataCheck = Data.Database.Reader.ReadOne(queryCheck, parametersCheck);
		if (dataCheck == null)
		{
			throw new StatusCodeException(404, "Could not find portfolio with id " + id);
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "DELETE FROM Portfolios WHERE uid = @uid";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@uid", id);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(409, "Could not delete portfolio with id " + id);
		}
	}

	public CashTransaction GetCashTransaction(int cashTransactionID)
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		String query = "SELECT * FROM CashTransactions WHERE id = @id AND portfolio = @portfolio";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", cashTransactionID);
		parameters.Add("@portfolio", id!);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);
		if (data != null)
		{
			CashTransaction cashTransaction = new CashTransaction();
			cashTransaction.id = int.Parse(data["id"].ToString()!);
			cashTransaction.portfolioId = data["portfolio"].ToString();
			cashTransaction.timestamp = Convert.ToInt32(data["timestamp"]);
			cashTransaction.nativeAmount = new Money(Convert.ToDecimal(data["amount_currency"]), data["currency"].ToString()!);
			cashTransaction.usdAmount = new Money(Convert.ToDecimal(data["amount_usd"]), "USD");
			cashTransaction.description = data["description"].ToString();
			return cashTransaction;
		}
		throw new StatusCodeException(404, "Could not find cash transaction with id " + cashTransactionID);
	}

	public void UpdateCashTransactions()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}

		String query = "SELECT * FROM CashTransactions WHERE portfolio = @portfolio";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", id!);
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(query, parameters);
		cashTransactions = new List<CashTransaction>();
		foreach (Dictionary<String, object> row in data)
		{
			CashTransaction cashTransaction = new CashTransaction();
			cashTransaction.id = int.Parse(row["id"].ToString()!);
			cashTransaction.portfolioId = row["portfolio"].ToString();
			cashTransaction.timestamp = Convert.ToInt32(row["timestamp"]);
			cashTransaction.nativeAmount = new Money(Convert.ToDecimal(row["amount_currency"]), row["currency"].ToString()!);
			cashTransaction.usdAmount = new Money(Convert.ToDecimal(row["amount_usd"]), "USD");
			cashTransaction.description = row["description"].ToString();
			cashTransactions.Add(cashTransaction);
		}
	}
}