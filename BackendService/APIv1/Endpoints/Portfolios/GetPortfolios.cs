using Microsoft.AspNetCore.Mvc;

namespace API.v1;

class GetPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/portfolios", ([FromQuery] string? id, string accessToken) =>
		{
			return Results.Ok(Endpoint(id, accessToken));
		});
	}

	public static GetPortfoliosResponse Endpoint(string id, string accessToken)
	{
		BusinessLogic.User user = new BusinessLogic.TokenSet(accessToken).GetUser();
		BusinessLogic.Portfolio portfolio = user.GetPortfolio(id);
		return new GetPortfoliosResponse("success", new List<BusinessLogic.Portfolio> { portfolio });
	}

	public static GetPortfoliosResponse Endpoint(string accessToken)
	{
		BusinessLogic.User user = new BusinessLogic.TokenSet(accessToken).GetUser();
		List<BusinessLogic.Portfolio> portfolios = user.GetPortfolios();
		return new GetPortfoliosResponse("success", portfolios);
	}
}