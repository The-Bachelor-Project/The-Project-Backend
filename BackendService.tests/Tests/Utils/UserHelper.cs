namespace BackendService.tests;
using StockApp;
using System.Data.SqlClient;
public class UserHelper
{
	public static User Create()
	{
		String email = Tools.RandomString.Generate(200) + "@test.com";
		String password = Tools.RandomString.Generate(200);
		PostUsersBody body = new PostUsersBody(email, password);
		PostUsersResponse response = PostUsers.Endpoint(body);
		User user = new User(email, password);
		user.id = response.uid!;
		return user;
	}

	public static void Delete(User user)
	{
		String deleteQuery = "DELETE FROM Accounts WHERE email = @email";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(deleteQuery, connection);
		command.Parameters.AddWithValue("@email", user.email);
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
