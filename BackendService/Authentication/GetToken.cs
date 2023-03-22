using System.Data.SqlClient;

namespace Authentication;

class GetToken
{
	public static GottenTokenResponse RefreshToken(String UserID)
	{
		String GetRefreshToken = "SELECT refresh_token FROM Tokens WHERE user_id = @user_id";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(GetRefreshToken, Connection);
		Command.Parameters.AddWithValue("@user_id", UserID);
		try
		{
			SqlDataReader Reader = Command.ExecuteReader();
			GottenTokenResponse RefreshTokenResponse = new GottenTokenResponse("", true);
			if (Reader.Read())
			{
				String RefreshToken = Reader["refresh_token"].ToString()!;
				RefreshTokenResponse.Token = RefreshToken;
				Reader.Close();
				return RefreshTokenResponse;
			}
			else
			{
				RefreshTokenResponse.Success = false;
				return RefreshTokenResponse;
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			GottenTokenResponse RefreshTokenResponse = new GottenTokenResponse("", false);
			return RefreshTokenResponse;
		}
	}

	public static GottenTokenResponse AccessToken(String RefreshToken)
	{
		String GetRefreshToken = "SELECT access_token FROM Tokens WHERE refresh_token = @refresh_token";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(GetRefreshToken, Connection);
		Command.Parameters.AddWithValue("@refresh_token", RefreshToken);
		try
		{
			SqlDataReader Reader = Command.ExecuteReader();
			GottenTokenResponse AccessTokenResponse = new GottenTokenResponse("", true);
			if (Reader.Read())
			{
				String AccessToken = Reader["access_token"].ToString()!;
				AccessTokenResponse.Token = RefreshToken;
				Reader.Close();
				return AccessTokenResponse;
			}
			else
			{
				AccessTokenResponse.Success = false;
				return AccessTokenResponse;
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			GottenTokenResponse AccessTokenResponse = new GottenTokenResponse("", false);
			return AccessTokenResponse;
		}
	}
}

class GottenTokenResponse
{

	public GottenTokenResponse(String Token, Boolean Success)
	{
		this.Token = Token;
		this.Success = Success;
	}

	public String Token { get; set; }
	public Boolean Success { get; set; }
}

