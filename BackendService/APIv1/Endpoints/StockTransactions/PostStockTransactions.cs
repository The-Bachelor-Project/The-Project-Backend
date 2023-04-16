namespace API.v1;

class PostStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/stock-transactions", (PostStockTransactionsBody body) =>
		{
			return Results.Ok(EndpointAsync(body));
		});
	}

	public static async Task<PostStockTransactionsResponse> EndpointAsync(PostStockTransactionsBody body)
	{
		BusinessLogic.User user = new BusinessLogic.TokenSet(body.accessToken).GetUser();
		System.Console.WriteLine("User: " + user.Id);
		BusinessLogic.StockTransaction StockTransaction = new BusinessLogic.StockTransaction();
		System.Console.WriteLine("----------------------------------");
		System.Console.WriteLine(body.stockTransaction.portfolio);
		StockTransaction.PortfolioId = body.stockTransaction.portfolio;
		StockTransaction.Ticker = body.stockTransaction.ticker;
		StockTransaction.Exchange = body.stockTransaction.exchange;
		StockTransaction.Amount = body.stockTransaction.amount;
		StockTransaction.Timestamp = body.stockTransaction.timestamp;
		StockTransaction.Currency = body.stockTransaction.currency;
		StockTransaction.Price = body.stockTransaction.price;
		await StockTransaction.AddToDb();
		if (StockTransaction.Id != null)
		{
			return new PostStockTransactionsResponse("success", StockTransaction.Id);
		}
		return new PostStockTransactionsResponse("error", null);
	}
}