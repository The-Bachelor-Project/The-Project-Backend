namespace StockApp;

using System.Data.SqlClient;
using BackendService;

public class TokenSet
{
	public TokenSet(String refreshToken, String accessToken)
	{
		this.refreshToken = refreshToken;
		this.accessToken = accessToken;
	}
	public TokenSet(String accessToken)
	{
		this.accessToken = accessToken;
	}
	public TokenSet()
	{
	}

	public String? refreshToken { get; set; }
	public String? accessToken { get; set; }

	public TokenSet SetRefreshToken(String refreshToken)
	{
		this.refreshToken = refreshToken;
		return this;
	}



	public static TokenSet Create(String uid)
	{
		TokenSet newTokenSet = new TokenSet();

		int familyID = Authentication.CreateFamily.Call();

		TokenSet tokenSet = Authentication.SetupTokens.Call(uid, familyID);
		newTokenSet.refreshToken = tokenSet.refreshToken;
		newTokenSet.accessToken = tokenSet.accessToken;

		return newTokenSet;
	}

	public TokenSet Refresh()
	{
		StockApp.TokenSet tokenSet = Authentication.RefreshTokens.All(refreshToken!);
		refreshToken = tokenSet.refreshToken;
		accessToken = tokenSet.accessToken;

		return this;
	}

	public User GetUser()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT user_id FROM Tokens WHERE access_token = @access_token";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@access_token", accessToken);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				User user = new User(reader["user_id"].ToString()!);
				reader.Close();
				return user;
			}
			reader.Close();
			throw new Exception("User not found");
		}
	}
}