namespace API.v1;
using Microsoft.AspNetCore.Mvc;
public class DeleteCashTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapDelete("/v1/cash-transactions", ([FromQuery] String portfolio, int id, HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new DeleteCashTransactionsResponse("error"));
			}
			return Results.Ok(Endpoint(portfolio, id, accessToken));
		});
	}

	public static DeleteCashTransactionsResponse Endpoint(String portfolioID, int id, String accessToken)
	{
		if (portfolioID == null || portfolioID == "" || accessToken == null || accessToken == "")
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		StockApp.Portfolio portfolio = user.GetPortfolio(portfolioID);
		StockApp.CashTransaction cashTransaction = portfolio.GetCashTransaction(id);
		cashTransaction.Delete();
		return new DeleteCashTransactionsResponse("success");
	}
}