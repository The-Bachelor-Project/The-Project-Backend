using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-transactions", ([FromQuery] string accessToken) =>
		{
			return Results.Ok(Endpoint(accessToken));
		});
	}

	public static GetStockTransactionsResponse Endpoint(string accessToken)
	{
		if (accessToken == null)
		{
			return new GetStockTransactionsResponse("error", new List<Data.StockTransaction>());
		}
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		List<Data.StockTransaction> stockTransactions = user.GetAllStockTransactions();
		return new GetStockTransactionsResponse("success", stockTransactions);
	}
}