using System.Data.SqlClient;

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
		StockApp.User user = new StockApp.TokenSet(body.accessToken).GetUser();
		System.Console.WriteLine("User: " + user.id);
		StockApp.User owner = new StockApp.Portfolio(body.transaction.portfolio).GetOwner();
		StockApp.StockTransaction StockTransaction = new StockApp.StockTransaction();
		StockTransaction.portfolioId = body.transaction.portfolio;
		StockTransaction.ticker = body.transaction.ticker;
		StockTransaction.exchange = body.transaction.exchange;
		StockTransaction.amount = body.transaction.amount;
		StockTransaction.timestamp = body.transaction.timestamp;
		StockTransaction.price = new StockApp.Money(body.transaction.price.amount, body.transaction.price.currency);
		await StockTransaction.AddToDb();
		if (StockTransaction.id != null)
		{
			return new PostStockTransactionsResponse("success", StockTransaction.id);
		}
		return new PostStockTransactionsResponse("error", null);
	}
}