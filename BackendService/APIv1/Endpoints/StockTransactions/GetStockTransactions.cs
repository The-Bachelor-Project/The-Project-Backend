using Microsoft.AspNetCore.Mvc;

namespace API.v1;

class GetStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-transactions", ([FromQuery] string id, string ticker, string exchange, string portfolio, string accessToken) =>
		{
			return Results.Ok(Endpoint(id, ticker, exchange, portfolio, accessToken));
		});
	}

	public static GetStockTransactionsResponse Endpoint(string id, string ticker, string exchange, string portfolio, string accessToken)
	{
		List<Data.StockTransaction> StockTransactions = new List<Data.StockTransaction>();
		StockTransactions.Add(new Data.StockTransaction(portfolio, ticker, exchange, (decimal)0.0, 1000, null!));
		return new GetStockTransactionsResponse("success", StockTransactions);
	}
}