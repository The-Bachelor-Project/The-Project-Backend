namespace API.v1.Endpoints;

class PostStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/stock-transactions", (PostStockTransactionsBody body) =>
		{
			return Results.Ok(Endpoint(body));
		});
	}

	public static PostStockTransactionsResponse Endpoint(PostStockTransactionsBody body)
	{
		return new PostStockTransactionsResponse("success", body.accessToken);
	}
}