using System.Data.SqlClient;

namespace Authentication;

class RefreshTokens
{
	public static RefreshTokensBody call(String UID, int familyID)
	{
		//TODO: Change the refresh from taking UID to refresh token instead and validate
		String RefreshToken = Tools.RandomString.Generate(128);
		String AccessToken = Tools.RandomString.Generate(128);
		int RefreshExpirationUnix = Authentication.Expiration.GenerateRefresh((24 * 7));
		int AccessExpirationUnix = Authentication.Expiration.GenerateAccess(30);
		String CreateTokenRecord = "INSERT INTO Tokens VALUES (@user_id, @refresh_token, @refresh_expiration, @access_token, @access_expiration, @family)";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(CreateTokenRecord, Connection);
		Command.Parameters.AddWithValue("@user_id", UID);
		Command.Parameters.AddWithValue("@refresh_token", RefreshToken);
		Command.Parameters.AddWithValue("@refresh_expiration", RefreshExpirationUnix);
		Command.Parameters.AddWithValue("@access_token", AccessToken);
		Command.Parameters.AddWithValue("@access_expiration", AccessExpirationUnix);
		Command.Parameters.AddWithValue("@family", familyID);
		try
		{
			Command.ExecuteNonQuery();
			RefreshTokensBody RefreshTokensBody = new RefreshTokensBody(true, RefreshToken, AccessToken);
			return RefreshTokensBody;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			RefreshTokensBody RefreshTokensBody = new RefreshTokensBody(false, "", "");
			return RefreshTokensBody;
		}
	}
}

public class RefreshTokensBody
{
	public RefreshTokensBody(Boolean success, String refresh, String acccess)
	{
		this.success = success;
		this.refresh = refresh;
		this.access = acccess;
	}
	public Boolean success { get; set; }
	public String refresh { get; set; }
	public String access { get; set; }
}