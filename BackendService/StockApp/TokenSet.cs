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

		int familyID = Authentication.CreateFamily.Call(uid);

		TokenSet tokenSet = Authentication.SetupTokens.Call(familyID);
		newTokenSet.refreshToken = tokenSet.refreshToken;
		newTokenSet.accessToken = tokenSet.accessToken;

		return newTokenSet;
	}

	public TokenSet Refresh(int familyID)
	{
		StockApp.TokenSet tokenSet = Authentication.RefreshTokens.All(refreshToken!, familyID);
		refreshToken = tokenSet.refreshToken;
		accessToken = tokenSet.accessToken;

		return this;
	}

	public User GetUser()
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "SELECT user_id FROM TokenFamily WHERE valid_token = @access_token";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@access_token", accessToken!);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);

		if (data != null)
		{
			User user = new User(data["user_id"].ToString()!);
			return user;
		}
		throw new UnauthorizedAccess("User not found");
	}
}