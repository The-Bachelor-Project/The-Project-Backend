namespace API.v1;

public class GetCashTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/cash-transactions", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetCashTransactionsResponse("error", new List<StockApp.CashTransaction> { }));
			}
			return Results.Ok(Endpoint(accessToken));
		});
	}

	public static GetCashTransactionsResponse Endpoint(String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		List<StockApp.CashTransaction> cashTransactions = user.GetAllCashTransactions();
		return new GetCashTransactionsResponse("success", cashTransactions);
	}
}