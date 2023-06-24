using System.Data.SqlClient;
namespace Authentication;
public class SetupTokens
{
	public static StockApp.TokenSet Call(int familyID)
	{
		int refreshExpirationUnix = Authentication.Expiration.GenerateRefresh((24 * 7));
		int accessExpirationUnix = Authentication.Expiration.GenerateAccess(60 * 10);
		String refreshToken = Tools.RandomString.Generate(128);
		String accessToken = Tools.RandomString.Generate(128);
		String createTokenRecord = "INSERT INTO Tokens VALUES (@refresh_token, @refresh_expiration, @access_token, @access_expiration, @family)";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(createTokenRecord, connection);
		command.Parameters.AddWithValue("@refresh_token", refreshToken);
		command.Parameters.AddWithValue("@refresh_expiration", refreshExpirationUnix);
		command.Parameters.AddWithValue("@access_token", accessToken);
		command.Parameters.AddWithValue("@access_expiration", accessExpirationUnix);
		command.Parameters.AddWithValue("@family", familyID);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to create token record");
		}
		Boolean successfulUpdate = UpdateValidToken(accessToken, familyID);
		if (successfulUpdate)
		{
			StockApp.TokenSet tokenSet = new StockApp.TokenSet(refreshToken, accessToken);
			return tokenSet;
		}
		else
		{
			throw new StatusCodeException(500, "Failed to update valid token");
		}
	}

	private static Boolean UpdateValidToken(String accessToken, int familyID)
	{
		String updateValidTokenQuery = "UPDATE TokenFamily SET valid_token = @accessToken WHERE id = @familyID";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(updateValidTokenQuery, connection);
		command.Parameters.AddWithValue("@accessToken", accessToken);
		command.Parameters.AddWithValue("@familyID", familyID);
		try
		{
			command.ExecuteNonQuery();
			return true;
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			return false;
		}
	}
}