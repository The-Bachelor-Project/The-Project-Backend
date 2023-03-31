namespace API.v1;
class PostPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/portfolios", (Data.Portfolio body) =>
		{
			return Results.Ok(Endpoint(body));
		});
	}

	public static PostPortfoliosResponse Endpoint(Data.Portfolio body)
	{
		return new PostPortfoliosResponse("success", "1234");
	}
}