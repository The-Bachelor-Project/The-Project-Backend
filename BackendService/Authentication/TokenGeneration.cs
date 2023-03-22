using System.Data.SqlClient;

namespace Authentication;

class TokenGeneration
{
	public static Boolean RefreshToken(String UID, String RefreshToken)
	{
		String CreateRefreshTokenRecord = "INSERT INTO Tokens (user_id, refresh_token) VALUES (@user_id, @refresh_token)";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(CreateRefreshTokenRecord, Connection);
		Command.Parameters.AddWithValue("@user_id", UID);
		Command.Parameters.AddWithValue("@refresh_token", RefreshToken);
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

	public static Boolean AccessToken(String RefreshToken)
	{

		String AccessToken = DatabaseService.RandomString.Generate(128);
		String UpdateAccessToken = "UPDATE Tokens SET access_token = @access_token WHERE refresh_token = @refresh_token";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(UpdateAccessToken, Connection);
		Command.Parameters.AddWithValue("@access_token", AccessToken);
		Command.Parameters.AddWithValue("@refresh_token", RefreshToken);
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