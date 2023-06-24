namespace API.v1;
public class PostPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/portfolios", (HttpContext context, PostPortfoliosBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(Endpoint(body, accessToken!));
		});
	}

	public static PostPortfoliosResponse Endpoint(PostPortfoliosBody body, String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		if (body.portfolio.name is null)
		{
			throw new StatusCodeException(400, "Missing name of portfolio");
		}
		if (body.portfolio.currency is null || body.portfolio.currency == "")
		{
			throw new StatusCodeException(400, "Missing currency of portfolio");
		}
		StockApp.Portfolio portfolio = new StockApp.Portfolio(body.portfolio.name, user.id!, body.portfolio.currency);
		portfolio.AddToDb();
		return new PostPortfoliosResponse("success", portfolio.id!);


	}
}