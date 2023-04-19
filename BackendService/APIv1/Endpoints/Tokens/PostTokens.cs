using StockApp;

namespace API.v1;

public class PostTokens
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
		TokenSet NewTokenSet = TokenSet.Create(new User(body.email, body.password).SignIn().Id!);
		return new TokensResponse("success", NewTokenSet);
	}
}
