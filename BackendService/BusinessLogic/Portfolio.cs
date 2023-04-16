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


	public StockTransaction[] GetStockTransactions()
	{
		throw new NotImplementedException();
	}
}