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
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String getEmailQuery = "SELECT email FROM Accounts WHERE email = @email";
		SqlCommand command = new SqlCommand(getEmailQuery, connection);
		command.Parameters.AddWithValue("@email", email);
		using (SqlDataReader reader = command.ExecuteReader())
		{
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
			reader.Close();
			throw new UserAlreadyExist();
		}
	}

	public User SignIn()
	{
		System.Console.WriteLine("SignIn with email: " + email);
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM Accounts WHERE email = @email";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@email", email);
		using (SqlDataReader reader = command.ExecuteReader())
		{
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
				return this;
			}
			reader.Close();
			throw new UserDoesNotExistException("No user with the email \"" + email + "\" was found");
		}
	}

	public User ChangeEmail(String oldEmail, String newEmail)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "UPDATE Accounts SET email = @new_email WHERE user_id = @user_id AND email = @old_email";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@user_id", id);
		command.Parameters.AddWithValue("@old_email", oldEmail);
		command.Parameters.AddWithValue("@new_email", newEmail);
		try
		{
			command.ExecuteNonQuery();
			this.email = email;
			return this;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new Exception();
		}
	}

	public User ChangePassword(String oldPassword, String newPassword, String email)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		oldPassword = Tools.Password.Hash(oldPassword);
		String query = "UPDATE Accounts SET password = @new_password WHERE user_id = @user_id AND email = @email AND password = @old_password";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@user_id", id);
		command.Parameters.AddWithValue("@old_password", oldPassword);
		command.Parameters.AddWithValue("@new_password", Tools.Password.Hash(newPassword));
		command.Parameters.AddWithValue("@email", email);
		try
		{
			command.ExecuteNonQuery();
			this.password = newPassword;
			return this;
		}
		catch (Exception)
		{
			throw new Exception();
		}
	}

	public void Delete()
	{
		throw new NotImplementedException();
	}

	public User UpdatePortfolios()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM Portfolios WHERE owner = @owner";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@owner", id);
		using (SqlDataReader reader = command.ExecuteReader())
		{
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
			reader.Close();
			return this;
		}

	}

	public Portfolio GetPortfolio(string id)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM Portfolios WHERE owner = @owner AND uid = @uid";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@owner", this.id);
		command.Parameters.AddWithValue("@uid", id);
		using (SqlDataReader reader = command.ExecuteReader())
		{
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
				reader.Close();
				return portfolio;
			}
			reader.Close();
			throw new Exception("Portfolio not found");
		}

	}

	public async Task<Data.UserAssetsValueHistory> GetValueHistory(string currency, DateOnly startData, DateOnly endDate)
	{
		UpdatePortfolios();

		List<Data.DatePriceOHLC> valueHistory = new List<Data.DatePriceOHLC>();
		List<Data.Portfolio> dataPortfolios = new List<Data.Portfolio>();
		List<Data.Dividend> dividendHistory = new List<Data.Dividend>();
		foreach (Portfolio portfolio in portfolios)
		{
			Data.Portfolio dataPortfolio = await portfolio.GetValueHistory(currency, startData, endDate);
			System.Console.WriteLine("COUNT USER: " + dataPortfolio.dividendHistory.Count);
			dataPortfolios.Add(dataPortfolio);
			valueHistory = Data.DatePriceOHLC.AddLists(valueHistory, dataPortfolio.valueHistory);
			dividendHistory.AddRange(dataPortfolio.dividendHistory);
		}

		return new Data.UserAssetsValueHistory(valueHistory, dataPortfolios, dividendHistory);
	}

	public List<Data.StockTransaction> GetAllStockTransactions()
	{
		UpdatePortfolios();

		List<Data.StockTransaction> transactions = new List<Data.StockTransaction>();

		foreach (Portfolio portfolio in portfolios)
		{
			portfolio.UpdateStockTransactions();
			foreach (StockTransaction transaction in portfolio.stockTransactions)
			{
				transactions.Add(new Data.StockTransaction(transaction.portfolioId!, transaction.ticker!, transaction.exchange!, transaction.amount ?? 0, transaction.timestamp ?? 0, new Data.Money(transaction.price!.amount, transaction.price.currency)));
			}
		}

		return transactions;
	}
}