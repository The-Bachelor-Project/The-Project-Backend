namespace API.v1;
using Microsoft.AspNetCore.Mvc;

public class GetCashTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/cash-transactions", ([FromQuery] String currency, HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetCashTransactionsResponse("error", new List<StockApp.CashTransaction> { }));
			}
			return Results.Ok(Endpoint(accessToken, currency));
		});
	}

	public static async Task<GetCashTransactionsResponse> Endpoint(String accessToken, String currency)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		List<StockApp.CashTransaction> cashTransactions = await user.GetAllCashTransactions(currency);
		return new GetCashTransactionsResponse("success", cashTransactions);
	}
}