using Microsoft.AspNetCore.Mvc;

namespace API.v1.Endpoints;

class GetTokens
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/tokens", ([FromQuery] string refreshToken) =>
		{
			return Results.Ok(Endpoint(refreshToken));
		});
	}

	public static TokensResponse Endpoint(string accessToken)
	{
		return new TokensResponse("success", new Data.TokenSet("44354gbvg20", "1111"));
	}
}