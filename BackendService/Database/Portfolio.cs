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

	public static Data.Portfolio[] GetAll(string owner)
	{
		SqlConnection Connection = Database.createConnection();
		String Query = "SELECT * FROM Portfolios WHERE owner = @owner";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@owner", owner);
		SqlDataReader Reader = Command.ExecuteReader();
		List<Data.Portfolio> portfolios = new List<Data.Portfolio>();
		while (Reader.Read())
		{
			Data.Portfolio portfolio = new Data.Portfolio(
				Reader.GetString(1),
				Reader.GetString(2),
				Reader.GetString(3),
				Reader.GetDecimal(4),
				Reader.GetBoolean(5)
			);
			portfolio.UID = Reader.GetString(0);
			portfolios.Add(portfolio);
		}
		return portfolios.ToArray();
	}
}