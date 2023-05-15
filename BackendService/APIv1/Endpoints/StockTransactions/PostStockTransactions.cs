using System.Data.SqlClient;

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
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
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
		System.Console.WriteLine("StockTransaction id: " + StockTransaction.id);
		if (StockTransaction.id != null)
		{
			return new PostStockTransactionsResponse("success", (int)StockTransaction.id);
		}
		return new PostStockTransactionsResponse("error", 0);
	}
}