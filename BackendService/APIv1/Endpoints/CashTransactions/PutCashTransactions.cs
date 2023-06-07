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
		StockApp.CashTransaction oldCashTransaction = portfolio.GetCashTransaction(body.id);
		StockApp.CashTransaction newCashTransaction = new StockApp.CashTransaction();
		newCashTransaction.portfolioId = oldCashTransaction.portfolioId;
		newCashTransaction.description = oldCashTransaction.description;
		newCashTransaction.nativeAmount = oldCashTransaction.nativeAmount;
		newCashTransaction.timestamp = oldCashTransaction.timestamp;
		newCashTransaction.usdAmount = oldCashTransaction.usdAmount;
		if (body.newDescription != "")
		{
			newCashTransaction.description = body.newDescription;
		}
		if (body.newNativeCurrency != "" && body.newNativeAmount != 0)
		{
			newCashTransaction.nativeAmount = new StockApp.Money(body.newNativeAmount, body.newNativeCurrency);
		}
		else if (body.newNativeCurrency != "")
		{
			newCashTransaction.nativeAmount = new StockApp.Money(newCashTransaction.nativeAmount!.amount, body.newNativeCurrency);
		}
		else if (body.newNativeAmount != 0)
		{
			newCashTransaction.nativeAmount = new StockApp.Money(body.newNativeAmount, newCashTransaction.nativeAmount!.currency);
		}
		if (body.newTimestamp != 0)
		{
			newCashTransaction.timestamp = body.newTimestamp;
		}
		await newCashTransaction.AddToDb();
		oldCashTransaction.Delete();
		return new PutCashTransactionsResponse("success", (int)newCashTransaction.id!);
	}
}