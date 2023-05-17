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
		String getEmailQuery = "SELECT email FROM Accounts WHERE email = @email";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@email", email);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getEmailQuery, parameters);

		if (data == null)
		{
			SqlConnection connection = Data.Database.Connection.GetSqlConnection();
			String uid = Tools.RandomString.Generate(32);
			String signUpQuery = "INSERT INTO Accounts (user_id, email, password) VALUES (@user_id, @email, @password)";
			SqlCommand command = new SqlCommand(signUpQuery, connection);
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
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM Accounts WHERE email = @email";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@email", email);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);


		if (data != null)
		{
			String dbPassword = data["password"].ToString()!;
			String userID = data["user_id"].ToString()!;
			//TODO Check password

			if (dbPassword != Tools.Password.Hash(password!))
			{
				throw new WrongPasswordException("The password is incorrect");
			}

			id = userID;
			return this;
		}
		throw new UserDoesNotExistException("No user with the email \"" + email + "\" was found");
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
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@owner", id);
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(query, parameters);

		portfolios = new List<Portfolio>();
		foreach (Dictionary<String, object> row in data)
		{
			Portfolio portfolio = new Portfolio(
				row["uid"].ToString()!,
				row["name"].ToString()!,
				row["owner"].ToString()!,
				row["currency"].ToString()!,
				Convert.ToDecimal(row["balance"]),
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			portfolios.Add(portfolio);
		}
		return this;
	}

	public Portfolio GetPortfolio(string id)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM Portfolios WHERE owner = @owner AND uid = @uid";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@owner", this.id);
		parameters.Add("@uid", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);

		List<Portfolio> portfolios = new List<Portfolio>();
		if (data != null)
		{
			Portfolio portfolio = new Portfolio(
				data["uid"].ToString()!,
				data["name"].ToString()!,
				data["owner"].ToString()!,
				data["currency"].ToString()!,
				Convert.ToDecimal(data["balance"]),
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			return portfolio;
		}
		throw new Exception("Portfolio not found");

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
				transactions.Add(new Data.StockTransaction(transaction.id!.Value, transaction.portfolioId!, transaction.ticker!, transaction.exchange!, transaction.amount ?? 0, transaction.timestamp ?? 0, new Data.Money(transaction.price!.amount, transaction.price.currency)));
			}
		}

		return transactions;
	}
}