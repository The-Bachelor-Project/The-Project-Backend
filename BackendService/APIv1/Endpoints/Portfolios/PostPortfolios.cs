namespace API.v1;
public class PostPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/portfolios", (HttpContext context, PostPortfoliosBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(Endpoint(body, accessToken));
		});
	}

	public static PostPortfoliosResponse Endpoint(PostPortfoliosBody body, String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		System.Console.WriteLine("User: " + user.id);
		if (body.portfolio.name is null)
		{
			throw new StatusCodeException(400, "Missing name of portfolio");
		}
		if (body.portfolio.currency is null || body.portfolio.currency == "")
		{
			throw new StatusCodeException(400, "Missing currency of portfolio");
		}
		StockApp.Portfolio portfolio = new StockApp.Portfolio(body.portfolio.name, user.id!, body.portfolio.currency, body.portfolio.trackBalance);
		portfolio.AddToDb();
		if (portfolio.id != null)
		{
			return new PostPortfoliosResponse("success", portfolio.id);
		}
		return new PostPortfoliosResponse("error", "");
	}
}