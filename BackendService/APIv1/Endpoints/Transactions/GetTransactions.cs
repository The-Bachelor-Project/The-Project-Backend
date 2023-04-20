using Microsoft.AspNetCore.Mvc;

namespace API.v1;

class GetTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/transactions", ([FromQuery] string accessToken) =>
		{
			return Results.Ok(Endpoint(accessToken));
		});
	}

	public static GetTransactionsResponse Endpoint(string accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		user.UpdatePortfolios();
		foreach (StockApp.Portfolio portfolio in user.portfolios)
		{
			portfolio.UpdateStockTransactions();
		}

		return new GetTransactionsResponse("success", user.portfolios);
	}
}