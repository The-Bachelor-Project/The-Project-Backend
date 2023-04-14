using System.Data.SqlClient;

namespace BusinessLogic;

public class User
{
	public String? Id { get; set; }
	public String? Email { get; set; }
	public String? Password { get; set; }

	public User(string email, string password)
	{
		this.Email = email;
		this.Password = password;
	}

	public User(string id)
	{
		this.Id = id;
	}


	public User SignUp()
	{
		try
		{
			Id = DatabaseService.User.SignUp(Email, Password);
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
		}

		return this;
	}

	public User SignIn()
	{
		Id = DatabaseService.User.GetUserId(Email, Password);
		return this;
	}

	public void Delete()
	{
		throw new NotImplementedException();
	}

	public List<Portfolio> GetPortfolios()
	{
		SqlConnection Connection = new Data.Database.Connection().Create();
		String Query = "SELECT * FROM Portfolios WHERE owner = @owner";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@owner", Id);
		SqlDataReader Reader = Command.ExecuteReader();
		List<Portfolio> portfolios = new List<Portfolio>();
		while (Reader.Read())
		{
			Portfolio portfolio = new Portfolio(
				Reader["uid"].ToString(),
				Reader["name"].ToString(),
				Reader["owner"].ToString(),
				Reader["currency"].ToString()!,
				Convert.ToDecimal(Reader["balance"]),
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			portfolios.Add(portfolio);
		}
		return portfolios;
	}

	public Portfolio GetPortfolio(string id)
	{
		SqlConnection Connection = new Data.Database.Connection().Create();
		String Query = "SELECT * FROM Portfolios WHERE owner = @owner AND uid = @uid";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@owner", Id);
		Command.Parameters.AddWithValue("@uid", id);
		SqlDataReader Reader = Command.ExecuteReader();
		List<Portfolio> portfolios = new List<Portfolio>();
		if (Reader.Read())
		{
			Portfolio portfolio = new Portfolio(
				Reader["uid"].ToString(),
				Reader["name"].ToString(),
				Reader["owner"].ToString(),
				Reader["currency"].ToString()!,
				Convert.ToDecimal(Reader["balance"]),
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			return portfolio;
		}
		throw new Exception("Portfolio not found");
	}
}