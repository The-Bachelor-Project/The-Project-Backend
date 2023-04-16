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
		BusinessLogic.User user = new BusinessLogic.TokenSet(body.accessToken).GetUser();
		System.Console.WriteLine("User: " + user.Id);
		BusinessLogic.Portfolio portfolio = new BusinessLogic.Portfolio(body.portfolio.Name, user.Id!, body.portfolio.Currency, body.portfolio.Balance, body.portfolio.TrackBalance);
		portfolio.AddToDb();
		if (portfolio.Id != null)
		{
			return new PostPortfoliosResponse("success", portfolio.Id);
		}
		return new PostPortfoliosResponse("error", "");
	}
}