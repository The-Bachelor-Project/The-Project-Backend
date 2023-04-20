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


	public Portfolio AddToDb()
	{
		SqlConnection connection = new Data.Database.Connection().Create();
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
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "SELECT owner FROM Portfolios WHERE uid = @uid";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@uid", id);
		SqlDataReader reader = command.ExecuteReader();
		reader.Read();
		return new User(reader["owner"].ToString()!);
	}


	public Portfolio UpdateStockTransactions()
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "SELECT * FROM StockTransactions WHERE portfolio = @portfolio";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@portfolio", id);
		SqlDataReader reader = command.ExecuteReader();
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

		return this;
	}
}