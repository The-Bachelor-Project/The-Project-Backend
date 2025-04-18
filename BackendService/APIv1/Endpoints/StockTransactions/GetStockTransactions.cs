using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-transactions", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(Endpoint(accessToken!));
		});
	}

	public static GetStockTransactionsResponse Endpoint(String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		List<StockApp.StockTransaction> stockTransactions = user.GetAllStockTransactions();
		return new GetStockTransactionsResponse("success", stockTransactions);
	}
}