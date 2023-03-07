using System.Data.SqlClient;

namespace Authentication;

class GetToken
{
	public static GrantTokenResponse GrantToken(String UserID)
	{
		String GetGrantToken = "SELECT grant_token FROM Tokens WHERE user_id = @user_id";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(GetGrantToken, Connection);
		Command.Parameters.AddWithValue("@user_id", UserID);
		try
		{
			SqlDataReader Reader = Command.ExecuteReader();
			GrantTokenResponse GrantTokenResponse = new GrantTokenResponse("", true);
			if (Reader.Read())
			{
				String GrantToken = Reader["grant_token"].ToString()!;
				GrantTokenResponse.GrantToken = GrantToken;
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
			GrantTokenResponse GrantTokenResponse = new GrantTokenResponse("", false);
			return GrantTokenResponse;
		}
	}
}

class GrantTokenResponse
{

	public GrantTokenResponse(String GrantToken, Boolean Success)
	{
		this.GrantToken = GrantToken;
		this.Success = Success;
	}

	public String GrantToken { get; set; }
	public Boolean Success { get; set; }
}

