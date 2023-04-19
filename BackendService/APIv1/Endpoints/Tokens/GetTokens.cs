using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace API.v1;

class GetTokens
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/tokens", ([FromQuery] string refreshToken) =>
		{
			return Results.Ok(Endpoint(refreshToken));
		});
	}

	public static TokensResponse Endpoint(string refreshToken)
	{
		TokenSet NewTokenSet = new TokenSet().SetRefreshToken(refreshToken).Refresh();
		return new TokensResponse("success", NewTokenSet);
	}
}