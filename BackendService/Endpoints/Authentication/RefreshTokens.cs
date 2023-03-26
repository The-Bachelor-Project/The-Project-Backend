namespace BackendService;

public class RefreshTokens
{
	public static RefreshTokensResponse endpoint(RefreshTokensBody body)
	{
		RefreshTokensResponse RefreshTokensReponse = Authentication.RefreshTokens.all(body.refresh_token);
		return RefreshTokensReponse;
	}
}

public class RefreshTokensResponse
{
	public RefreshTokensResponse(String response, String refreshToken, String accessToken)
	{
		this.response = response;
		this.refreshToken = refreshToken;
		this.accessToken = accessToken;
	}
	public String response { get; set; }
	public String refreshToken { get; set; }
	public String accessToken { get; set; }
}

public class RefreshTokensBody
{
	public RefreshTokensBody(String refresh_token)
	{
		this.refresh_token = refresh_token;
	}
	public String refresh_token { get; set; }
}