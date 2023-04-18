using System.Data.SqlClient;

namespace BusinessLogic;

public class Portfolio
{

	public Portfolio(string id)
	{
		this.Id = id;
	}
	public Portfolio(string name, string owner, string currency, decimal balance, bool trackBalance)
	{
		this.Name = name;
		this.Owner = owner;
		this.Currency = currency;
		this.Balance = balance;
		this.TrackBalance = trackBalance;
	}
	public Portfolio(string id, string name, string owner, string currency, decimal balance, bool trackBalance)
	{
		this.Id = id;
		this.Name = name;
		this.Owner = owner;
		this.Currency = currency;
		this.Balance = balance;
		this.TrackBalance = trackBalance;
	}

	public String? Id { get; set; }
	public String? Name { get; set; }
	public String? Owner { get; set; }
	public String? Currency { get; set; }
	public Decimal? Balance { get; set; }
	public Boolean? TrackBalance { get; set; }

	public List<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();


	public Portfolio AddToDb()
	{
		SqlConnection Connection = new Data.Database.Connection().Create();
		Id = Tools.RandomString.Generate(32);
		String Query = "INSERT INTO Portfolios (uid, name, owner, currency, balance) VALUES (@uid, @name, @owner, @currency, @balance)";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@uid", Id);
		Command.Parameters.AddWithValue("@name", Name);
		Command.Parameters.AddWithValue("@owner", Owner);
		Command.Parameters.AddWithValue("@currency", Currency);
		Command.Parameters.AddWithValue("@balance", Balance);
		Command.ExecuteNonQuery();

		return this;
	}

	public User GetOwner()
	{
		SqlConnection Connection = new Data.Database.Connection().Create();
		String Query = "SELECT owner FROM Portfolios WHERE uid = @uid";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@uid", Id);
		SqlDataReader Reader = Command.ExecuteReader();
		Reader.Read();
		return new User(Reader["owner"].ToString()!);
	}


	public Portfolio UpdateStockTransactions()
	{
		SqlConnection Connection = new Data.Database.Connection().Create();
		String Query = "SELECT * FROM StockTransactions WHERE portfolio = @portfolio";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@portfolio", Id);
		SqlDataReader Reader = Command.ExecuteReader();
		StockTransactions = new List<StockTransaction>();
		while (Reader.Read())
		{
			StockTransactions.Add(new StockTransaction());
			StockTransactions.Last().Id = Reader["uid"].ToString();
			StockTransactions.Last().PortfolioId = Id;
			StockTransactions.Last().Ticker = Reader["ticker"].ToString();
			StockTransactions.Last().Exchange = Reader["exchange"].ToString();
			StockTransactions.Last().Amount = Convert.ToDecimal(Reader["amount"]);
			StockTransactions.Last().AmountAdjusted = Convert.ToDecimal(Reader["amount_adjusted"]);
			StockTransactions.Last().AmountOwned = Convert.ToDecimal(Reader["amount_owned"]);
			StockTransactions.Last().Timestamp = Convert.ToInt32(Reader["timestamp"]);
			StockTransactions.Last().Currency = Reader["currency"].ToString();
			StockTransactions.Last().Price = Convert.ToDecimal(Reader["price"]);
		}

		return this;
	}
}