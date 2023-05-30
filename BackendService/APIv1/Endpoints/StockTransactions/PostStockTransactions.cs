namespace API.v1;

public class PostStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/stock-transactions", (HttpContext context, PostStockTransactionsBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(EndpointAsync(body, accessToken));
		});
	}

	public static async Task<PostStockTransactionsResponse> EndpointAsync(PostStockTransactionsBody body, String accessToken)
	{
		if (body.transaction.portfolioId is null || body.transaction.portfolioId == "")
		{
			throw new StatusCodeException(400, "Missing portfolio id");
		}
		if (body.transaction.ticker is null || body.transaction.ticker == "")
		{
			throw new StatusCodeException(400, "Missing ticker");
		}
		if (body.transaction.exchange is null || body.transaction.exchange == "")
		{
			throw new StatusCodeException(400, "Missing exchange");
		}
		if (body.transaction.amount is null || body.transaction.amount == 0)
		{
			throw new StatusCodeException(400, "Missing amount. Can not be 0");
		}
		if (body.transaction.timestamp is null || body.transaction.timestamp == 0)
		{
			throw new StatusCodeException(400, "Missing timestamp");
		}
		if (body.transaction.price!.currency is null || body.transaction.price!.currency == "")
		{
			throw new StatusCodeException(400, "Missing currency of price");
		}
		if (body.transaction.price!.amount < 0)
		{
			throw new StatusCodeException(400, "Invalid price");
		}
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		System.Console.WriteLine("User: " + user.id);
		StockApp.User owner = new StockApp.Portfolio(body.transaction.portfolioId).GetOwner();
		if (user.id != owner.id)
		{
			throw new StatusCodeException(403, "User is not owner of portfolio");
		}

		StockApp.StockTransaction stockTransaction = new StockApp.StockTransaction();
		stockTransaction.portfolioId = body.transaction.portfolioId;
		stockTransaction.ticker = body.transaction.ticker;
		stockTransaction.exchange = body.transaction.exchange;
		stockTransaction.amount = body.transaction.amount;
		stockTransaction.timestamp = body.transaction.timestamp;
		stockTransaction.price = new StockApp.Money(body.transaction.price.amount, body.transaction.price.currency);
		await stockTransaction.AddToDb();
		System.Console.WriteLine("StockTransaction id: " + stockTransaction.id);
		if (stockTransaction.id != null)
		{
			return new PostStockTransactionsResponse("success", (int)stockTransaction.id);
		}
		return new PostStockTransactionsResponse("error", 0);
	}
}