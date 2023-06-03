namespace API.v1;

public class PutCashTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapPut("/v1/cash-transactions", (PutCashTransactionsBody body, HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new PutCashTransactionsResponse("error", -1));
			}
			return Results.Ok(Endpoint(body, accessToken));
		});
	}

	public async static Task<PutCashTransactionsResponse> Endpoint(PutCashTransactionsBody body, String accessToken)
	{
		if (accessToken == null || accessToken == "" || body.portfolio == "" || body.portfolio == null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		StockApp.Portfolio portfolio = user.GetPortfolio(body.portfolio);
		StockApp.CashTransaction cashTransaction = portfolio.GetCashTransaction(body.id);
		cashTransaction.DeleteFromDb();
		if (body.newDescription != "")
		{
			cashTransaction.description = body.newDescription;
		}
		if (body.newNativeCurrency != "" && body.newNativeAmount != 0)
		{
			cashTransaction.nativeAmount = new StockApp.Money(body.newNativeAmount, body.newNativeCurrency);
		}
		else if (body.newNativeCurrency != "")
		{
			cashTransaction.nativeAmount = new StockApp.Money(cashTransaction.nativeAmount!.amount, body.newNativeCurrency);
		}
		else if (body.newNativeAmount != 0)
		{
			cashTransaction.nativeAmount = new StockApp.Money(body.newNativeAmount, cashTransaction.nativeAmount!.currency);
		}
		if (body.newTimestamp != 0)
		{
			cashTransaction.timestamp = body.newTimestamp;
		}
		await cashTransaction.AddToDb();
		return new PutCashTransactionsResponse("success", (int)cashTransaction.id!);
	}
}