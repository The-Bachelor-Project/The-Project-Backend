using System.Data.SqlClient;
namespace Authentication;
class SetupTokens
{
	public static BackendService.RefreshTokensResponse call(String UID, int familyID)
	{
		int RefreshExpirationUnix = Authentication.Expiration.GenerateRefresh((24 * 7));
		int AccessExpirationUnix = Authentication.Expiration.GenerateAccess(30);
		String RefreshToken = Tools.RandomString.Generate(128);
		String AccessToken = Tools.RandomString.Generate(128);
		String CreateTokenRecord = "INSERT INTO Tokens VALUES (@user_id, @refresh_token, @refresh_expiration, @access_token, @access_expiration, @family)";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(CreateTokenRecord, Connection);
		Command.Parameters.AddWithValue("@user_id", UID);
		Command.Parameters.AddWithValue("@refresh_token", RefreshToken);
		System.Console.WriteLine(RefreshExpirationUnix);
		Command.Parameters.AddWithValue("@refresh_expiration", RefreshExpirationUnix);
		Command.Parameters.AddWithValue("@access_token", AccessToken);
		Command.Parameters.AddWithValue("@access_expiration", AccessExpirationUnix);
		Command.Parameters.AddWithValue("@family", familyID);
		try
		{
			Command.ExecuteNonQuery();
			BackendService.RefreshTokensResponse RefreshTokensResponse = new BackendService.RefreshTokensResponse("success", RefreshToken, AccessToken);
			return RefreshTokensResponse;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			BackendService.RefreshTokensResponse RefreshTokensResponse = new BackendService.RefreshTokensResponse("error", "", "");
			return RefreshTokensResponse;
		}
	}
}