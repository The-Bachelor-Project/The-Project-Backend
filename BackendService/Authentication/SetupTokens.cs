using System.Data.SqlClient;
namespace Authentication;
class SetupTokens
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
		System.Console.WriteLine(refreshExpirationUnix);
		command.Parameters.AddWithValue("@refresh_expiration", refreshExpirationUnix);
		command.Parameters.AddWithValue("@access_token", accessToken);
		command.Parameters.AddWithValue("@access_expiration", accessExpirationUnix);
		command.Parameters.AddWithValue("@family", familyID);
		try
		{
			command.ExecuteNonQuery();
			Boolean successfulUpdate = UpdateValidToken(accessToken, familyID);
			if (successfulUpdate)
			{
				StockApp.TokenSet tokenSet = new StockApp.TokenSet(refreshToken, accessToken);
				return tokenSet;
			}
			else
			{
				//FIXME: Error not being handled
				StockApp.TokenSet tokenSet = new StockApp.TokenSet("error", "");
				return tokenSet;
			}

		}
		catch (Exception e)
		{
			//FIXME: Error not being handled
			System.Console.WriteLine(e);
			StockApp.TokenSet tokenSet = new StockApp.TokenSet("error", "");
			return tokenSet;
		}
	}

	private static Boolean UpdateValidToken(String accessToken, int familyID)
	{
		String updateValidRefreshQuery = "UPDATE TokenFamily SET valid_token = @accessToken WHERE id = @familyID";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(updateValidRefreshQuery, connection);
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