using System.Data.SqlClient;

namespace DatabaseService;

class Portfolio
{
	public static void Add(Data.Portfolio portfolio)
	{
		SqlConnection Connection = Database.createConnection();
		String UID = RandomString.Generate(32);
		String Query = "INSERT INTO Portfolios (uid, name, owner, currency, balance) VALUES (@uid, @name, @owner, @currency, @balance)";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@uid", UID);
		Command.Parameters.AddWithValue("@name", portfolio.Name);
		Command.Parameters.AddWithValue("@owner", portfolio.Owner);
		Command.Parameters.AddWithValue("@currency", portfolio.Currency);
		Command.Parameters.AddWithValue("@balance", portfolio.Balance);
		Command.ExecuteNonQuery();
	}
}