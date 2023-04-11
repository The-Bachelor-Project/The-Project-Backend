namespace BusinessLogic;

using BackendService;

public class TokenSet
{
	public TokenSet(String refreshToken)
	{
		RefreshToken = refreshToken;
		AccessToken = "";
	}
	public TokenSet()
	{
	}

	public String? RefreshToken { get; set; }
	public String? AccessToken { get; set; }

	public static TokenSet Create(String uid){
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
}