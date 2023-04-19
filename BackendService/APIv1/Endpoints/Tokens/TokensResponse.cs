using BusinessLogic;

namespace API.v1;

public class TokensResponse
{
	public TokensResponse(string response, TokenSet tokenSet)
	{
		this.response = response;
		this.tokenSet = tokenSet;
	}

	public string response { get; }
	public TokenSet tokenSet { get; set; }
}