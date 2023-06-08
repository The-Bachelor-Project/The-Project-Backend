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
		if (email == null || password == null)
		{
			throw new StatusCodeException(400, "Fields are missing");
		}
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
				throw new StatusCodeException(500, "Failed to create user");
			}
			return this;
		}
		throw new StatusCodeException(409, "User with email: " + email + " already exist");
	}

	public User SignIn()
	{
		if (email == null || password == null)
		{
			throw new StatusCodeException(400, "Fields are missing");
		}
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
				throw new StatusCodeException(401, "The password is incorrect");
			}
			id = userID;
			return this;
		}
		throw new StatusCodeException(404, "No user with the email: " + email + " was found");
	}

	public User ChangeEmail(String newEmail)
	{
		if (newEmail == null)
		{
			throw new StatusCodeException(400, "Fields are missing");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "UPDATE Accounts SET email = @new_email WHERE user_id = @user_id";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@user_id", id);
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
			throw new StatusCodeException(409, "Could not update user email");
		}
	}

	public User ChangePassword(String oldPassword, String newPassword)
	{
		if (oldPassword == null || newPassword == null)
		{
			throw new StatusCodeException(400, "Fields are missing");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		oldPassword = Tools.Password.Hash(oldPassword);
		String getOldPasswordQuery = "SELECT password FROM Accounts WHERE password = @password";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@password", oldPassword);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getOldPasswordQuery, parameters);
		if (data == null)
		{
			throw new StatusCodeException(401, "The password is incorrect");
		}
		String updatePasswordQuery = "UPDATE Accounts SET password = @new_password WHERE user_id = @user_id AND password = @old_password";
		SqlCommand command = new SqlCommand(updatePasswordQuery, connection);
		command.Parameters.AddWithValue("@user_id", id);
		command.Parameters.AddWithValue("@old_password", oldPassword);
		command.Parameters.AddWithValue("@new_password", Tools.Password.Hash(newPassword));
		try
		{
			command.ExecuteNonQuery();
			this.password = newPassword;
			return this;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(409, "Could not update user password");
		}
	}

	public void Delete()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "DELETE FROM Accounts WHERE user_id = @user_id";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@user_id", id);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(409, "Could not delete user");
		}
	}

	public User UpdatePortfolios()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Fields are missing");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM Portfolios WHERE owner = @owner";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@owner", id);
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(query, parameters);
		if (data == null)
		{
			throw new StatusCodeException(404, "No portfolios found");
		}
		portfolios = new List<Portfolio>();
		foreach (Dictionary<String, object> row in data)
		{
			Portfolio portfolio = new Portfolio(
				row["uid"].ToString()!,
				row["name"].ToString()!,
				row["owner"].ToString()!,
				row["currency"].ToString()!,
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			portfolios.Add(portfolio);
		}
		return this;
	}

	public Portfolio GetPortfolio(string id)
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Fields are missing");
		}
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
				true //Convert.ToBoolean(Reader["track_balance"]) //TODO add to database to it can be used here
			);
			return portfolio;
		}
		throw new StatusCodeException(404, "Portfolio with id " + id + " not found");

	}

	public async Task<Data.UserAssetsValueHistory> GetValueHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		UpdatePortfolios();
		List<Data.Transaction> allTransactions = await GetTransactions(currency);
		int firstIndex = allTransactions.FindLastIndex(x => x.timestamp <= Tools.TimeConverter.DateOnlyToUnix(startDate));
		int lastIndex = allTransactions.FindLastIndex(x => x.timestamp <= Tools.TimeConverter.DateOnlyToUnix(endDate));
		if (firstIndex == -1)
		{
			firstIndex = 0;
		}
		List<Data.Transaction> newTransactions = new List<Data.Transaction>();
		if (lastIndex != -1)
		{
			newTransactions = allTransactions.GetRange(firstIndex < 0 ? 0 : firstIndex, ((lastIndex - firstIndex + 1) < 0 ? 0 : (lastIndex - firstIndex + 1)));
		}


		List<Data.DatePriceOHLC> valueHistory = new List<Data.DatePriceOHLC>();
		List<Data.Portfolio> dataPortfolios = new List<Data.Portfolio>();
		List<Data.Dividend> dividendHistory = new List<Data.Dividend>();
		List<Data.CashBalance> cashBalance = new List<Data.CashBalance>();
		foreach (Portfolio portfolio in portfolios)
		{
			Data.Portfolio dataPortfolio = await portfolio.GetValueHistory(currency, startDate, endDate);
			dataPortfolios.Add(dataPortfolio);
			valueHistory = Data.DatePriceOHLC.AddLists(valueHistory, dataPortfolio.valueHistory);
			dividendHistory.AddRange(dataPortfolio.dividendHistory);
		}
		cashBalance.AddRange(newTransactions.Select(transaction => new Data.CashBalance(
			Tools.TimeConverter.UnixTimeStampToDateOnly(transaction.timestamp),
			transaction.combinedBalance
		)).ToList());

		return new Data.UserAssetsValueHistory(valueHistory, dataPortfolios, dividendHistory, InsertMissingValues(cashBalance));
	}

	public List<Data.CashBalance> InsertMissingValues(List<Data.CashBalance> cashBalances)
	{
		List<Data.CashBalance> newCashBalances = new List<Data.CashBalance>();
		for (int i = 0; i < cashBalances.Count - 1; i++)
		{
			Data.CashBalance current = cashBalances[i];
			Data.CashBalance next = cashBalances[i + 1];

			newCashBalances.Add(current);

			DateOnly currentDate = current.date;
			DateOnly nextDate = next.date;

			while (currentDate.AddDays(1) < nextDate)
			{
				currentDate = currentDate.AddDays(1);
				Data.CashBalance missingCashBalance = new Data.CashBalance(currentDate, current.balance);
				newCashBalances.Add(missingCashBalance);
			}
		}

		return newCashBalances;
	}

	public async Task<List<Data.Transaction>> GetTransactions(String currency)
	{
		String getTransactionsQuery = "SELECT * FROM AllTransactions WHERE owner = @owner ORDER BY timestamp ASC, transaction_type ASC, portfolio ASC, id ASC";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@owner", id!);
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(getTransactionsQuery, parameters);

		List<Data.Transaction> transactions = new List<Data.Transaction>();
		Money balance = new Money(0, "USD");
		Dictionary<String, Money> portfolioBalances = new Dictionary<string, Money>();
		List<int> timestamps = new List<int>();
		data.ForEach(row => timestamps.Add((int)row["timestamp"]));
		List<Decimal> exchangeRates = await Tools.PriceConverter.GetExchangeHistory(timestamps, currency);
		int i = 0;
		foreach (Dictionary<String, object> row in data)
		{
			String type = row["transaction_type"].ToString()!;
			String portfolio = row["portfolio"].ToString()!;
			int timestamp = Convert.ToInt32(row["timestamp"]);
			int id = Convert.ToInt32(row["id"]);
			String description;
			Money amount;

			switch (type)
			{
				case "DividendPayout":
					amount = new Money(Convert.ToDecimal(row["payout"]) * Convert.ToDecimal(row["shares_amount"]), "USD");
					break;
				default:
					amount = new Money(Convert.ToDecimal(row["amount_usd"]), "USD");
					break;
			}

			balance.amount += amount.amount;
			if (portfolioBalances.ContainsKey(portfolio))
			{
				portfolioBalances[portfolio].amount += amount.amount;
			}
			else
			{
				portfolioBalances.Add(portfolio, new Money(amount.amount, amount.currency));
			}

			switch (type)
			{
				case "CashTransaction":
					description = row["description"].ToString()!;
					break;
				default:
					description = row["shares_amount"].ToString()! + " " + row["exchange"].ToString()! + " " + row["ticker"].ToString()!;
					break;
			}

			transactions.Add(new Data.Transaction(
				type,
				id,
				portfolio,
				timestamp,
				description,
				new Money(amount.amount * (1 / exchangeRates[i]), currency),
				new Money(portfolioBalances[portfolio].amount * (1 / exchangeRates[i]), currency),
				new Money(balance.amount * (1 / exchangeRates[i]), currency))
				);
			i++;
		}
		return transactions;
	}

	public List<StockApp.StockTransaction> GetAllStockTransactions()
	{
		UpdatePortfolios();

		List<StockApp.StockTransaction> transactions = new List<StockApp.StockTransaction>();

		foreach (Portfolio portfolio in portfolios)
		{
			portfolio.UpdateStockTransactions();
			foreach (StockTransaction transaction in portfolio.stockTransactions)
			{
				transactions.Add(new StockApp.StockTransaction(transaction.id!.Value, transaction.portfolioId!, transaction.ticker!, transaction.exchange!, transaction.amount, transaction.timestamp, new StockApp.Money(transaction.priceNative!.amount, transaction.priceNative.currency)));
			}
		}

		return transactions;
	}

	public int PostPreference(string setting, string value)
	{
		if (id == null || setting == null || value == null)
		{
			throw new StatusCodeException(400, "Required fields are missing");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String insertQuery = @" MERGE INTO AccountPreferences AS AP
								USING (SELECT @user_id AS user_id, @setting AS setting, @value AS value) AS original
								ON AP.user_id = original.user_id AND AP.setting = original.setting
								WHEN MATCHED THEN
									UPDATE SET value = original.value
								WHEN NOT MATCHED THEN
									INSERT (user_id, setting, value) VALUES (original.user_id, original.setting, original.value)
								OUTPUT INSERTED.id;
							";
		SqlCommand command = new SqlCommand(insertQuery, connection);
		command.Parameters.AddWithValue("@user_id", id);
		command.Parameters.AddWithValue("@setting", setting);
		command.Parameters.AddWithValue("@value", value);
		try
		{
			return (int)command.ExecuteScalar();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not add preference");
		}
	}

	public Dictionary<string, string> GetPreferences()
	{
		if (id == null)
		{
			throw new StatusCodeException(400, "Required fields are missing");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT * FROM AccountPreferences WHERE user_id = @user_id";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@user_id", id);
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(query, parameters);
		if (data == null || data.Count == 0)
		{
			return new Dictionary<string, string>();
		}
		Dictionary<string, string> preferences = new Dictionary<string, string>();
		foreach (Dictionary<String, object> row in data)
		{
			preferences.Add(row["setting"].ToString()!, row["value"].ToString()!);
		}
		return preferences;
	}

	public void DeletePreference(string setting)
	{
		if (id == null || setting == null)
		{
			throw new StatusCodeException(400, "Required fields are missing");
		}
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String deleteQuery = "DELETE FROM AccountPreferences WHERE user_id = @user_id AND setting = @setting";
		SqlCommand command = new SqlCommand(deleteQuery, connection);
		command.Parameters.AddWithValue("@user_id", id);
		command.Parameters.AddWithValue("@setting", setting);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not delete preference");
		}
	}

	public async Task<List<CashTransaction>> GetAllCashTransactions(String currency)
	{
		UpdatePortfolios();

		List<CashTransaction> transactions = new List<CashTransaction>();

		foreach (Portfolio portfolio in portfolios)
		{
			portfolio.UpdateCashTransactions();
			foreach (CashTransaction transaction in portfolio.cashTransactions)
			{
				CashTransaction cashTransaction = new CashTransaction();
				cashTransaction.portfolioId = transaction.portfolioId;
				cashTransaction.nativeAmount = transaction.nativeAmount;
				cashTransaction.timestamp = transaction.timestamp;
				cashTransaction.description = transaction.description;
				cashTransaction.id = transaction.id;
				cashTransaction.usdAmount = transaction.usdAmount;
				transactions.Add(cashTransaction);
			}
		}

		return transactions;
	}
}