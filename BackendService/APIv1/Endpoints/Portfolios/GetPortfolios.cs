namespace API.v1;
using Microsoft.AspNetCore.Mvc;

public class GetPortfolios
{
	public static void Setup(WebApplication app, [FromQuery] String? id = null)
	{
		app.MapGet("/v1/portfolios", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (id is not null)
			{
				return Results.Ok(Endpoint(id, accessToken!));
			}
			return Results.Ok(Endpoint(accessToken!));
		});
	}

	public static GetPortfoliosResponse Endpoint(string id, string accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		StockApp.Portfolio portfolio = user.GetPortfolios(id);
		return new GetPortfoliosResponse("success", new List<StockApp.Portfolio> { portfolio });
	}

	public static GetPortfoliosResponse Endpoint(string accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		List<StockApp.Portfolio> portfolios = user.UpdatePortfolios().portfolios;
		return new GetPortfoliosResponse("success", portfolios);
	}
}