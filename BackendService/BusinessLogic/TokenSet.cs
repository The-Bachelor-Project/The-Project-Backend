namespace BusinessLogic;

using System.Data.SqlClient;
using BackendService;

public class TokenSet
{
	public TokenSet(String accessToken)
	{
		AccessToken = accessToken;
	}
	public TokenSet()
	{
	}

	public String? RefreshToken { get; set; }
	public String? AccessToken { get; set; }

	public TokenSet SetRefreshToken(String refreshToken)
	{
		RefreshToken = refreshToken;
		return this;
	}

	public static TokenSet Create(String uid)
	{
		TokenSet NewTokenSet = new TokenSet();

		int FamilyID = Authentication.CreateFamily.call();

		RefreshTokensResponse RefreshTokensResponse = Authentication.SetupTokens.call(uid, FamilyID);
		NewTokenSet.RefreshToken = RefreshTokensResponse.refreshToken;
		NewTokenSet.AccessToken = RefreshTokensResponse.response;

		return NewTokenSet;
	}

	public TokenSet Refresh()
	{
		BackendService.RefreshTokensResponse RefreshTokensResponse = Authentication.RefreshTokens.all(RefreshToken!);
		RefreshToken = RefreshTokensResponse.refreshToken;
		AccessToken = RefreshTokensResponse.accessToken;
		return this;
	}

	public User GetUser()
	{
		SqlConnection Connection = new Data.Database.Connection().Create();
		String Query = "SELECT user_id FROM Tokens WHERE access_token = @access_token";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@access_token", AccessToken);
		SqlDataReader Reader = Command.ExecuteReader();
		Reader.Read();
		return new User(Reader["user_id"].ToString());
	}
}