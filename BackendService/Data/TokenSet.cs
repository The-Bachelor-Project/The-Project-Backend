namespace Data;

public class TokenSet
{
	public TokenSet(String refreshToken, String accessToken)
	{
		RefreshToken = refreshToken;
		AccessToken = accessToken;
	}

	public String RefreshToken { get; set; }
	public String AccessToken { get; set; }
}