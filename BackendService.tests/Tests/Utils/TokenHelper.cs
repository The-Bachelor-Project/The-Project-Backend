namespace BackendService.tests;
using System.Data.SqlClient;

public class TokenHelper
{
	public static void MakeTokensExpired(String accessToken, String refreshToken)
	{
		String updateTokensQuery = "UPDATE Tokens SET refresh_expiration = @expiration, access_expiration = @expiration WHERE refresh_token = @refreshToken AND access_token = @accessToken";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(updateTokensQuery, connection);
		command.Parameters.AddWithValue("@expiration", 0);
		command.Parameters.AddWithValue("@refreshToken", refreshToken);
		command.Parameters.AddWithValue("@accessToken", accessToken);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to make tokens expired");
		}
	}
}