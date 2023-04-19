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
		StockApp.User user = new StockApp.TokenSet(body.accessToken).GetUser();
		System.Console.WriteLine("User: " + user.Id);
		StockApp.Portfolio portfolio = new StockApp.Portfolio(body.portfolio.Name, user.Id!, body.portfolio.Currency, body.portfolio.Balance, body.portfolio.TrackBalance);
		portfolio.AddToDb();
		if (portfolio.Id != null)
		{
			return new PostPortfoliosResponse("success", portfolio.Id);
		}
		return new PostPortfoliosResponse("error", "");
	}
}