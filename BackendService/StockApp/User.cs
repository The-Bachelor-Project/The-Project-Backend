using System.Data.SqlClient;

namespace StockApp;

public class User
{
	public String? id { get; set; }
	public String? email { get; set; }
	public String? password { get; set; }

	public List<Portfolio> portfolios = new List<Portfolio>();

	public User(string email, string password)
	{
		this.email = email;
		this.password = password;
	}

	public User(string id)
	{
		this.id = id;
	}


	public User SignUp()
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String getEmailQuery = "SELECT email FROM Accounts WHERE email = @email";
		SqlCommand command = new SqlCommand(getEmailQuery, connection);
		command.Parameters.AddWithValue("@email", email);
		SqlDataReader reader = command.ExecuteReader();
		if (!reader.Read())
		{
			reader.Close();
			String uid = Tools.RandomString.Generate(32);
			String signUpQuery = "INSERT INTO Accounts (user_id, email, password) VALUES (@user_id, @email, @password)";
			command = new SqlCommand(signUpQuery, connection);
			command.Parameters.AddWithValue("@user_id", uid);
			command.Parameters.AddWithValue("@email", email);
			command.Parameters.AddWithValue("@password", Tools.Password.Hash(password!));
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
			}
			return this;
		}
		throw new UserAlreadyExist();
	}

	public User SignIn()
	{
		System.Console.WriteLine("SignIn with email: " + email);
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "SELECT * FROM Accounts WHERE email = @email";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@email", email);
		SqlDataReader reader = command.ExecuteReader();
		if (reader.Read())
		{
			String dbPassword = reader["password"].ToString()!;
			String userID = reader["user_id"].ToString()!;
			reader.Close();
			//TODO Check password

			if (dbPassword != Tools.Password.Hash(password!))
			{
				throw new WrongPasswordException("The password is incorrect");
			}

			id = userID;
		}
		else
		{
			throw new UserDoesNotExistException("No user with the email \"" + email + "\" was found");
		}
		return this;
	}

	public void Delete()
	{
		throw new NotImplementedException();
	}

	public User UpdatePortfolios()
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "SELECT * FROM Portfolios WHERE owner = @owner";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@owner", id);
		SqlDataReader reader = command.ExecuteReader();
		portfolios = new List<Portfolio>();
		while (reader.Read())
		{
			Portfolio portfolio = new Portfolio(
				reader["uid"].ToString()!,
				reader["name"].ToString()!,
				reader["owner"].ToString()!,
				reader["currency"].ToString()!,
				Convert.ToDecimal(reader["balance"]),
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			portfolios.Add(portfolio);
		}
		return this;
	}

	public Portfolio GetPortfolio(string id)
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "SELECT * FROM Portfolios WHERE owner = @owner AND uid = @uid";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@owner", this.id);
		command.Parameters.AddWithValue("@uid", id);
		SqlDataReader reader = command.ExecuteReader();
		List<Portfolio> portfolios = new List<Portfolio>();
		if (reader.Read())
		{
			Portfolio portfolio = new Portfolio(
				reader["uid"].ToString()!,
				reader["name"].ToString()!,
				reader["owner"].ToString()!,
				reader["currency"].ToString()!,
				Convert.ToDecimal(reader["balance"]),
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			return portfolio;
		}
		throw new Exception("Portfolio not found");
	}

	public List<Data.DatePrice> GetValueHistory(string currency, DateOnly startData, DateOnly endDate)
	{
		throw new NotImplementedException();
	}
}