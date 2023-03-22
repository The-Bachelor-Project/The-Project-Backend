using System.Data.SqlClient;

namespace DatabaseService;

class Portfolio
{
	public static void Create(Data.Portfolio portfolio)
	{
		SqlConnection Connection = Database.createConnection();
		String UID = Tools.RandomString.Generate(32);
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
				Reader["name"].ToString(),
				Reader["owner"].ToString(),
				Reader["currency"].ToString()!,
				Convert.ToDecimal(Reader["balance"]),
				true
			//Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			portfolio.UID = Reader["uid"].ToString();
			portfolios.Add(portfolio);
		}
		return portfolios.ToArray();
	}
}