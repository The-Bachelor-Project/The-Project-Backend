using System.Data.SqlClient;

namespace StockApp;

public class Portfolio
{

	public Portfolio(string id)
	{
		this.id = id;
	}
	public Portfolio(string name, string owner, string currency, decimal balance, bool trackBalance)
	{
		this.name = name;
		this.owner = owner;
		this.currency = currency;
		this.balance = balance;
		this.trackBalance = trackBalance;
	}
	public Portfolio(string id, string name, string owner, string currency, decimal balance, bool trackBalance)
	{
		this.id = id;
		this.name = name;
		this.owner = owner;
		this.currency = currency;
		this.balance = balance;
		this.trackBalance = trackBalance;
	}

	public String? id { get; set; }
	public String? name { get; set; }
	public String? owner { get; set; }
	public String? currency { get; set; }
	public Decimal? balance { get; set; }
	public Boolean? trackBalance { get; set; }

	public List<StockTransaction> stockTransactions { get; set; } = new List<StockTransaction>();
	public List<StockPosition> stockPositions { get; set; } = new List<StockPosition>();



	public Portfolio AddToDb()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		id = Tools.RandomString.Generate(32);
		String query = "INSERT INTO Portfolios (uid, name, owner, currency, balance) VALUES (@uid, @name, @owner, @currency, @balance)";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@uid", id);
		command.Parameters.AddWithValue("@name", name);
		command.Parameters.AddWithValue("@owner", owner);
		command.Parameters.AddWithValue("@currency", currency);
		command.Parameters.AddWithValue("@balance", balance);
		command.ExecuteNonQuery();

		return this;
	}

	public User GetOwner()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT owner FROM Portfolios WHERE uid = @uid";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@uid", id);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				String userId = reader["owner"].ToString()!;
				User user = new User(userId);
				reader.Close();
				return user;
			}
			reader.Close();
			throw new Exception();
		}

	}


	public Portfolio UpdateStockTransactions()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM StockTransactions WHERE portfolio = @portfolio";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@portfolio", id);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			stockTransactions = new List<StockTransaction>();
			while (reader.Read())
			{
				stockTransactions.Add(new StockTransaction());
				stockTransactions.Last().id = reader["id"].ToString();
				stockTransactions.Last().portfolioId = id;
				stockTransactions.Last().ticker = reader["ticker"].ToString();
				stockTransactions.Last().exchange = reader["exchange"].ToString();
				stockTransactions.Last().amount = Convert.ToDecimal(reader["amount"]);
				stockTransactions.Last().amountAdjusted = Convert.ToDecimal(reader["amount_adjusted"]);
				stockTransactions.Last().amountOwned = Convert.ToDecimal(reader["amount_owned"]);
				stockTransactions.Last().timestamp = Convert.ToInt32(reader["timestamp"]);
				stockTransactions.Last().price = new Money(Convert.ToDecimal(reader["price_amount"]), reader["price_currency"].ToString()!);
			}
			reader.Close();
			return this;
		}
	}

	public Portfolio UpdateStockPositions()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT DISTINCT ticker, exchange FROM StockTransactions WHERE portfolio = @portfolio";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@portfolio", id);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			stockPositions = new List<StockPosition>();
			while (reader.Read())
			{
				stockPositions.Add(new StockPosition(this, new Stock(reader["ticker"].ToString()!, reader["exchange"].ToString()!)));
				System.Console.WriteLine(stockPositions.Last().stock.ticker + " " + stockPositions.Last().stock.exchange);
			}
			reader.Close();
			return this;
		}
	}

	public async Task<Data.Portfolio> GetValueHistory(string currency, DateOnly startData, DateOnly endDate)
	{
		UpdateStockPositions();
		UpdateStockTransactions();

		List<Data.DatePriceOHLC> valueHistory = new List<Data.DatePriceOHLC>();
		List<Data.Position> dataPositions = new List<Data.Position>();
		List<Data.Dividend> dividendHistory = new List<Data.Dividend>();

		foreach (StockPosition position in stockPositions)
		{
			Data.Position dataPosition = await position.GetValueHistory(currency, startData, endDate);
			if (dataPosition.valueHistory.Count > 0)
			{
				dataPositions.Add(dataPosition);
				valueHistory = Data.DatePriceOHLC.AddLists(valueHistory, dataPosition.valueHistory);
				dividendHistory.AddRange(dataPosition.dividends);
			}
		}
		System.Console.WriteLine("RETURNED: " + dividendHistory.Count);
		return new Data.Portfolio("[NAME]"/* TODO Get name */, currency, valueHistory, dataPositions, dividendHistory);
	}

	public Portfolio ChangeName(string newName)
	{
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
			throw new Exception();
		}
	}

	public Portfolio ChangeCurrency(String newCurrency)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String getCurrencyQuery = "SELECT * FROM Currencies WHERE code = @currency";
		SqlCommand command = new SqlCommand(getCurrencyQuery, connection);
		command.Parameters.AddWithValue("@currency", newCurrency);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			if (!reader.Read())
			{
				reader.Close();
				throw new Exception();
			}
			reader.Close();
		}
		String updateCurrency = "UPDATE Portfolios SET currency = @currency WHERE uid = @uid";
		command = new SqlCommand(updateCurrency, connection);
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
			throw new Exception();
		}
	}
}