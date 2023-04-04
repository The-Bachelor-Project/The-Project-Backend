namespace API.v1.Endpoints;

class PostTokens
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/tokens", (PostTokensBody body) =>
		{
			return Results.Ok(Endpoint(body));
		});
	}

	public static TokensResponse Endpoint(PostTokensBody body)
	{
		return new TokensResponse("success", new Data.TokenSet("4420", "1111"));
	}
}
