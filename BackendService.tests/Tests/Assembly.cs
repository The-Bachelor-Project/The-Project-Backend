namespace BackendService.tests;

using System.Data.SqlClient;

[TestClass]
public class Assembly
{
	public static StockApp.User? user = null;
	public static String accessToken = "";
	public static String portfolioID = "";
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
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "DELETE FROM Portfolios WHERE uid = @id";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@id", portfolioID);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (System.Exception)
		{
			System.Console.WriteLine("Portfolio could not be deleted");
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