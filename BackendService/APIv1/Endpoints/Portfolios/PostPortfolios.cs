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
		BusinessLogic.Portfolio portfolio = new BusinessLogic.Portfolio(body.portfolio.Name, body.portfolio.Owner, body.portfolio.Currency, body.portfolio.Balance, body.portfolio.TrackBalance);
		PostPortfoliosResponse response = new PostPortfoliosResponse("success", portfolio.Id);

		return response;
	}
}