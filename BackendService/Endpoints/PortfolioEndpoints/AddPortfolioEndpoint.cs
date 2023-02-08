using System.Data.SqlClient;

namespace BackendService;

class AddPortfolio
{
	public static AddPortfolioResponse endpoint(AddPortfolioBody body)
	{
		AddPortfolioResponse addPortfolioResponse = new AddPortfolioResponse("error");
		if (ValidateUserToken.authenticate(body.token))
		{
			using (SqlConnection connection = Database.createConnection())
			{
				String uid = RandomStringGenerator.Generate(32);
				String query = "INSERT INTO Portfolios (uid, name, owner, currency, balance) VALUES (@uid, @name, @owner, @currency, @balance)";
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@uid", uid);
				command.Parameters.AddWithValue("@name", body.name);
				command.Parameters.AddWithValue("@owner", body.owner);
				command.Parameters.AddWithValue("@currency", body.currency);
				command.Parameters.AddWithValue("@balance", body.balance);
				try
				{
					command.ExecuteNonQuery();
					addPortfolioResponse.response = "success";
				}
				catch (Exception)
				{
					addPortfolioResponse.response = "error";
				}
			}
		}
		else
		{
			addPortfolioResponse.response = "User not logged in";
		}


		return addPortfolioResponse;
	}
}

class AddPortfolioResponse
{
	public AddPortfolioResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class AddPortfolioBody
{
	public AddPortfolioBody(string name, string owner, string currency, decimal balance, bool trackBalance, string token)
	{
		this.name = name;
		this.owner = owner;
		this.currency = currency;
		this.balance = balance;
		this.trackBalance = trackBalance;
		this.token = token;
	}

	public String name { get; set; }
	public String owner { get; set; }
	public String currency { get; set; }
	public Decimal balance { get; set; }
	public Boolean trackBalance { get; set; }
	public String token { get; set; }
}