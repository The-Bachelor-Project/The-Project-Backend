using System.Data.SqlClient;

namespace Authentication;

class GetToken
{
	public static GottenTokenResponse GrantToken(String UserID)
	{
		String GetGrantToken = "SELECT grant_token FROM Tokens WHERE user_id = @user_id";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(GetGrantToken, Connection);
		Command.Parameters.AddWithValue("@user_id", UserID);
		try
		{
			SqlDataReader Reader = Command.ExecuteReader();
			GottenTokenResponse GrantTokenResponse = new GottenTokenResponse("", true);
			if (Reader.Read())
			{
				String GrantToken = Reader["grant_token"].ToString()!;
				GrantTokenResponse.Token = GrantToken;
				Reader.Close();
				return GrantTokenResponse;
			}
			else
			{
				GrantTokenResponse.Success = false;
				return GrantTokenResponse;
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			GottenTokenResponse GrantTokenResponse = new GottenTokenResponse("", false);
			return GrantTokenResponse;
		}
	}

	public static GottenTokenResponse RefreshToken(String GrantToken)
	{
		String GetGrantToken = "SELECT refresh_token FROM Tokens WHERE grant_token = @grant_token";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(GetGrantToken, Connection);
		Command.Parameters.AddWithValue("@grant_token", GrantToken);
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
			GottenTokenResponse GrantTokenResponse = new GottenTokenResponse("", false);
			return GrantTokenResponse;
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

