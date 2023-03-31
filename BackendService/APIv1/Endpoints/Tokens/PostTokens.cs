namespace API.v1;

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
		return new TokensResponse("success", "1234","1234435436");
	}
}
