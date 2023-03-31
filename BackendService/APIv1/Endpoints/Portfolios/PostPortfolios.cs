namespace API.v1;
class PostPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/portfolios", (PostPortfoliosBody body) =>
		{
			return Results.Ok(Endpoint(body));
		});
	}

	public static PostPortfoliosResponse Endpoint(PostPortfoliosBody body)
	{
		return new PostPortfoliosResponse("success", body.accessToken);
	}
}