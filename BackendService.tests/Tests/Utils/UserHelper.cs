namespace BackendService.tests;
using StockApp;
using System.Data.SqlClient;
public class UserHelper
{
	public static UserTestObject Create()
	{
		String email = Tools.RandomString.Generate(200) + "@test.com";
		String password = Tools.RandomString.Generate(200);
		PostUsersBody body = new PostUsersBody(email, password);
		PostUsersResponse response = PostUsers.Endpoint(body);
		User user = new User(email, password);
		user.id = response.uid!;
		PostTokensBody tokenBody = new PostTokensBody(email, password);
		StockApp.TokenSet tokenResponse = PostTokens.Endpoint(tokenBody);
		return new UserTestObject(user, tokenResponse.accessToken!, tokenResponse.refreshToken!);
	}

	public static void Delete(UserTestObject userTestObject)
	{
		String deleteQuery = "DELETE FROM Accounts WHERE email = @email";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(deleteQuery, connection);
		command.Parameters.AddWithValue("@email", userTestObject.user!.email);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
		}
	}
}

public class UserTestObject
{
	public User? user { get; set; }
	public String? accessToken { get; set; }
	public String? refreshToken { get; set; }
	public UserTestObject(User user, String accessToken, String refreshToken)
	{
		this.user = user;
		this.accessToken = accessToken;
		this.refreshToken = refreshToken;
	}
	public UserTestObject()
	{

	}
}
