namespace BackendService.tests;

using System.Data.SqlClient;

[TestClass]
public class Assembly
{
	public static StockApp.User? user = null;
	public static String accessToken = "";
	public static String refreshToken = "";
	public static String[] portfolioIds = new String[4];
	public static String email = "";
	public static String password = "";

	[AssemblyInitialize]
	public static void Initialize(TestContext context)
	{
		SignInHelper.Reset();
		SetupUser();
	}

	private static void SetupUser()
	{
		accessToken = SignInHelper.GetAccessToken();
		refreshToken = SignInHelper.GetRefreshToken();
		user = new StockApp.TokenSet(accessToken).GetUser();
		email = SignUpHelper.GetEmail();
		password = SignUpHelper.GetPassword();
	}

	[AssemblyCleanup]
	public static void Cleanup()
	{
		DeletePortfolio();
		DeleteUser();
	}

	private static void DeletePortfolio()
	{
		foreach (String id in portfolioIds)
		{
			SqlConnection connection = new Data.Database.Connection().Create();
			String query = "DELETE FROM Portfolios WHERE uid = @id";
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@id", id);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (System.Exception)
			{
				System.Console.WriteLine("Portfolio with id " + id + " could not be deleted");
			}
			connection.Close();
		}
	}

	private static void DeleteUser()
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "DELETE FROM Accounts WHERE email = @email";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@email", email);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (System.Exception)
		{
			System.Console.WriteLine("User could not be deleted");
		}
	}
}