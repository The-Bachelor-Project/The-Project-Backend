namespace API.v1;

public class PostCashTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/cash-transactions", async (HttpContext context, PostCashTransactionsBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return await Endpoint(body, accessToken!);
		});
	}

	public static async Task<PostCashTransactionsResponse> Endpoint(PostCashTransactionsBody body, String accessToken)
	{
		if (body.currency is null || body.currency == "" || body.nativeAmount == 0 || body.timestamp == 0 || body.portfolio is null || body.portfolio == "")
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		StockApp.User owner = new StockApp.Portfolio(body.portfolio).GetOwner();
		if (user.id != owner.id)
		{
			throw new StatusCodeException(403, "User is not owner of portfolio");
		}
		StockApp.CashTransaction cashTransaction = new StockApp.CashTransaction(body.portfolio, new StockApp.Money(body.nativeAmount, body.currency), body.timestamp, body.description);
		await cashTransaction.AddToDb();
		return new PostCashTransactionsResponse("success", (int)cashTransaction.id!);
	}
}