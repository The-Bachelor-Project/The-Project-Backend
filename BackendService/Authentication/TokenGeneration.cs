using System.Data.SqlClient;

namespace Authentication;

class TokenGeneration
{
	public static Boolean GrantToken(String UID, String GrantToken)
	{
		String CreateGrantTokenRecord = "INSERT INTO Tokens (user_id, grant_token) VALUES (@user_id, @grant_token)";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(CreateGrantTokenRecord, Connection);
		Command.Parameters.AddWithValue("@user_id", UID);
		Command.Parameters.AddWithValue("@grant_token", GrantToken);
		try
		{
			Command.ExecuteNonQuery();
			return true;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			return false;
		}
	}

	public static Boolean RefreshToken(String GrantToken)
	{
		String RefreshToken = DatabaseService.RandomString.Generate(128);
		String UpdateRefreshToken = "UPDATE Tokens SET refresh_token = @refresh_token WHERE grant_token = @grant_token";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(UpdateRefreshToken, Connection);
		Command.Parameters.AddWithValue("@refresh_token", RefreshToken);
		Command.Parameters.AddWithValue("@grant_token", GrantToken);
		try
		{
			Command.ExecuteNonQuery();
			return true;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			return false;
		}
	}
}