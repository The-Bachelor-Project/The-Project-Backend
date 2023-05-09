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
		TokenSet newTokenSet = TokenSet.Create(new User(body.email, body.password).SignIn().id!);
		return new TokensResponse("success", newTokenSet);
	}
}
