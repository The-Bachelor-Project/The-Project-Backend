using Microsoft.AspNetCore.Mvc;

namespace API.v1;

class GetTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-transactions", ([FromQuery] string accessToken) =>
		{
			return Results.Ok(Endpoint(accessToken));
		});
	}

	public static GetTransactionsResponse Endpoint(string accessToken)
	{
		BusinessLogic.User user = new BusinessLogic.TokenSet(accessToken).GetUser();
		user.UpdatePortfolios();
		foreach (BusinessLogic.Portfolio portfolio in user.Portfolios)
		{
			portfolio.UpdateStockTransactions();
		}

		return new GetTransactionsResponse("success", user.Portfolios);
	}
}