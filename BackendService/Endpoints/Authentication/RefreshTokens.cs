namespace BackendService;

public class RefreshTokens
{
	public static RefreshTokensResponse endpoint(RefreshTokensBody body)
	{
		RefreshTokensResponse RefreshTokensResponse = new RefreshTokensResponse("response", "refresh", "access");


		return RefreshTokensResponse;
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
	public RefreshTokensBody(String refreshToken)
	{
		this.refreshToken = refreshToken;
	}
	public String refreshToken { get; set; }
}