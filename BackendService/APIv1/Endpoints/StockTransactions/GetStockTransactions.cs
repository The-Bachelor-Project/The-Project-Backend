using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-transactions", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(Endpoint(accessToken));
		});
	}

	public static GetStockTransactionsResponse Endpoint(string accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		List<StockApp.StockTransaction> stockTransactions = user.GetAllStockTransactions();
		return new GetStockTransactionsResponse("success", stockTransactions);
	}
}