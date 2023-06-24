namespace API.v1;

public class PostStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/stock-transactions", async (HttpContext context, PostStockTransactionsBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return await Endpoint(body, accessToken!);
		});
	}

	public static async Task<PostStockTransactionsResponse> Endpoint(PostStockTransactionsBody body, String accessToken)
	{
		if (body.portfolioId is null || body.portfolioId == "")
		{
			throw new StatusCodeException(400, "Missing portfolio id");
		}
		if (body.ticker is null || body.ticker == "")
		{
			throw new StatusCodeException(400, "Missing ticker");
		}
		if (body.exchange is null || body.exchange == "")
		{
			throw new StatusCodeException(400, "Missing exchange");
		}
		if (body.amount == 0)
		{
			throw new StatusCodeException(400, "Missing amount. Can not be 0");
		}
		if (body.timestamp == 0)
		{
			throw new StatusCodeException(400, "Missing timestamp");
		}
		if (body.priceNative!.currency is null || body.priceNative!.currency == "")
		{
			throw new StatusCodeException(400, "Missing currency of price");
		}
		if (body.priceNative!.amount < 0)
		{
			throw new StatusCodeException(400, "Invalid price");
		}

		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		StockApp.User owner = new StockApp.Portfolio(body.portfolioId).GetOwner();
		if (user.id != owner.id)
		{
			throw new StatusCodeException(403, "User is not owner of portfolio");
		}
		StockApp.StockTransaction transaction = new StockApp.StockTransaction();
		transaction.portfolioId = body.portfolioId;
		transaction.ticker = body.ticker;
		transaction.exchange = body.exchange;
		transaction.amount = body.amount;
		transaction.timestamp = body.timestamp;
		transaction.priceNative = body.priceNative;
		await transaction.AddToDb();
		return new PostStockTransactionsResponse("success", (int)transaction.id!);
	}
}