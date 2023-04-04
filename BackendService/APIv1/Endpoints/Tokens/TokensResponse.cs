namespace API.v1.Endpoints;

public class TokensResponse
{
	public TokensResponse(string response, Data.TokenSet tokenSet)
	{
		this.response = response;
		this.tokenSet = tokenSet;
	}

	public string response { get; }
	public Data.TokenSet tokenSet { get; set; }
}