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
		BusinessLogic.User user = new BusinessLogic.TokenSet(body.accessToken).GetUser();
		System.Console.WriteLine("User: " + user.Id);
		BusinessLogic.User Owner = new BusinessLogic.Portfolio(body.transaction.portfolio).GetOwner();
		BusinessLogic.StockTransaction StockTransaction = new BusinessLogic.StockTransaction();
		StockTransaction.PortfolioId = body.transaction.portfolio;
		StockTransaction.Ticker = body.transaction.ticker;
		StockTransaction.Exchange = body.transaction.exchange;
		StockTransaction.Amount = body.transaction.amount;
		StockTransaction.Timestamp = body.transaction.timestamp;
		StockTransaction.Currency = body.transaction.currency;
		StockTransaction.Price = body.transaction.price;
		await StockTransaction.AddToDb();
		if (StockTransaction.Id != null)
		{
			return new PostStockTransactionsResponse("success", StockTransaction.Id);
		}
		return new PostStockTransactionsResponse("error", null);
	}
}