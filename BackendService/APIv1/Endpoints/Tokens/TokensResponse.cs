namespace API.v1;

public class TokensResponse
{
	public TokensResponse(string response, string refreshToken, string accessToken)
	{
		this.response = response;
		this.refreshToken = refreshToken;
		this.accessToken = accessToken;
	}

	public string response { get; }
	public string refreshToken { get; }
	public string accessToken { get; }
}