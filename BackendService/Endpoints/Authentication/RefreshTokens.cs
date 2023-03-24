namespace BackendService;

class RefreshTokens
{
	public static void endpoint(RefreshTokensBody body)
	{

	}
}

class RefreshTokensResponse
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