namespace API.v1;
public class PostPortfolios
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
		System.Console.WriteLine("User: " + user.id);
		StockApp.Portfolio portfolio = new StockApp.Portfolio(body.portfolio.name, user.id!, body.portfolio.currency, body.portfolio.balance, body.portfolio.trackBalance);
		portfolio.AddToDb();
		if (portfolio.id != null)
		{
			return new PostPortfoliosResponse("success", portfolio.id);
		}
		return new PostPortfoliosResponse("error", "");
	}
}