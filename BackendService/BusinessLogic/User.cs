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
		SqlConnection Connection = new Data.Database.Connection().Create();
		String GetEmailQuery = "SELECT email FROM Accounts WHERE email = @email";
		SqlCommand Command = new SqlCommand(GetEmailQuery, Connection);
		Command.Parameters.AddWithValue("@email", Email);
		SqlDataReader Reader = Command.ExecuteReader();
		if (!Reader.Read())
		{
			Reader.Close();
			String UID = Tools.RandomString.Generate(32);
			String SignUpQuery = "INSERT INTO Accounts (user_id, email, password) VALUES (@user_id, @email, @password)";
			Command = new SqlCommand(SignUpQuery, Connection);
			Command.Parameters.AddWithValue("@user_id", UID);
			Command.Parameters.AddWithValue("@email", Email);
			Command.Parameters.AddWithValue("@password", Tools.Password.Hash(Password!));
			try
			{
				Command.ExecuteNonQuery();
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
		System.Console.WriteLine("SignIn with email: " + Email);
		SqlConnection Connection = new Data.Database.Connection().Create();
		String Query = "SELECT * FROM Accounts WHERE email = @email";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@email", Email);
		SqlDataReader Reader = Command.ExecuteReader();
		if (Reader.Read())
		{
			String DbPassword = Reader["password"].ToString()!;
			String UserID = Reader["user_id"].ToString()!;
			Reader.Close();
			//TODO Check password

			if (DbPassword != Tools.Password.Hash(Password!))
			{
				throw new WrongPasswordException("The password is incorrect");
			}

			Id = UserID;
		}
		else
		{
			throw new UserDoesNotExistException("No user with the email \"" + Email + "\" was found");
		}
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
				Reader["uid"].ToString()!,
				Reader["name"].ToString()!,
				Reader["owner"].ToString()!,
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
				Reader["uid"].ToString()!,
				Reader["name"].ToString()!,
				Reader["owner"].ToString()!,
				Reader["currency"].ToString()!,
				Convert.ToDecimal(Reader["balance"]),
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			return portfolio;
		}
		throw new Exception("Portfolio not found");
	}
}