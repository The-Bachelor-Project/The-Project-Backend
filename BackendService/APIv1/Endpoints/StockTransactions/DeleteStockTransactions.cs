namespace API.v1;
using StockApp;
using Microsoft.AspNetCore.Mvc;

public class DeleteStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapDelete("/v1/stock-transactions", (HttpContext context, [FromQuery] String portfolio, int id) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(Endpoint(accessToken!, portfolio, id));
		});
	}

	public async static Task<DeleteStockTransactionsResponse> Endpoint(String accessToken, String portfolioID, int transactionID)
	{
		User user = new TokenSet(accessToken).GetUser();
		Portfolio portfolio = user.GetPortfolios(portfolioID);
		StockTransaction stockTransaction = portfolio.GetStockTransaction(transactionID);
		await stockTransaction.Delete();
		return new DeleteStockTransactionsResponse("success");
	}
}