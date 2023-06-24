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
		if (refreshToken == null)
		{
			throw new StatusCodeException(400, "Refresh token cannot be null");
		}

		this.refreshToken = refreshToken;
		return this;
	}


	/// <summary>
	/// Creates a new TokenSet for the specified user.
	/// </summary>
	/// <param name="uid">The user ID to create the TokenSet.</param>
	/// <returns>The newly created <see cref="TokenSet"/>.</returns>
	public static TokenSet Create(String uid)
	{
		if (uid == null)
		{
			throw new StatusCodeException(400, "User id cannot be null");
		}
		TokenSet newTokenSet = new TokenSet();

		int familyID = Authentication.CreateFamily.Call(uid);

		TokenSet tokenSet = Authentication.SetupTokens.Call(familyID);
		newTokenSet.refreshToken = tokenSet.refreshToken;
		newTokenSet.accessToken = tokenSet.accessToken;

		return newTokenSet;
	}

	/// <summary>
	/// Refreshes the access token using the refresh token.
	/// </summary>
	/// <returns>The <see cref="TokenSet"/> with the new access and refresh token.</returns>
	public TokenSet Refresh()
	{
		if (refreshToken == null)
		{
			throw new StatusCodeException(400, "Tokens cannot be null");
		}
		StockApp.TokenSet tokenSet = Authentication.RefreshTokens.All(refreshToken!);
		refreshToken = tokenSet.refreshToken;
		accessToken = tokenSet.accessToken;

		return this;
	}

	/// <summary>
	/// Retrieves the user who has the access token.
	/// </summary>
	/// <returns>The <see cref="User"/> object who has the access token.</returns>
	public User GetUser()
	{
		if (accessToken == null)
		{
			throw new StatusCodeException(400, "Access token cannot be null");
		}
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
		throw new StatusCodeException(401, "User not found");
	}
}