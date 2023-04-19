using BusinessLogic;

namespace API.v1;

public class TokensResponse
{
	public TokensResponse(string response, TokenSet tokenSet)
	{
		this.Response = response;
		this.TokenSet = tokenSet;
	}

	public string Response { get; }
	public TokenSet TokenSet { get; set; }
}