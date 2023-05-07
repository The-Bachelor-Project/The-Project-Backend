using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/portfolios", (HttpContext context, [FromQuery] string? id) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}

			if (id is null || id == "")
				return Results.Ok(Endpoint(accessToken));
			else
				return Results.Ok(Endpoint(id, accessToken));
		});
	}

	public static GetPortfoliosResponse Endpoint(string id, string accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		StockApp.Portfolio portfolio = user.GetPortfolio(id);
		return new GetPortfoliosResponse("success", new List<StockApp.Portfolio> { portfolio });
	}

	public static GetPortfoliosResponse Endpoint(string accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		List<StockApp.Portfolio> portfolios = user.UpdatePortfolios().portfolios;
		return new GetPortfoliosResponse("success", portfolios);
	}
}