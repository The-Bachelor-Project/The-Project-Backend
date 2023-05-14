namespace API.v1;
using StockApp;

public class PutStockTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapPut("/v1/stock-transactions", (HttpContext context, PutStockTransactionsBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new PutStockTransactionsResponse("error", ""));
			}
			return Results.Ok(Endpoint(accessToken, body));
		});
	}

	public async static Task<PutStockTransactionsResponse> Endpoint(string accessToken, PutStockTransactionsBody body)
	{
		if (body.id is null || body.portfolio is null)
		{
			return new PutStockTransactionsResponse("error", "");
		}
		try
		{
			User user = new TokenSet(accessToken).GetUser();
			Portfolio portfolio = user.GetPortfolio(body.portfolio);
			StockTransaction stockTransaction = portfolio.GetStockTransaction(body.id);
			await stockTransaction.DeleteFromDb();
			if (body.newAmount != 0)
			{
				stockTransaction.amount = body.newAmount;
			}

			if (body.newPrice != 0 && body.newCurrency != "")
			{
				stockTransaction.price = new Money(body.newPrice, body.newCurrency);
			}
			else if (body.newPrice != 0)
			{
				stockTransaction.price = new Money(body.newPrice, stockTransaction.price!.currency);
			}
			else if (body.newCurrency != "")
			{
				stockTransaction.price = new Money(stockTransaction.price!.amount, body.newCurrency);
			}

			if (body.newTimestamp != 0)
			{
				stockTransaction.timestamp = body.newTimestamp;
			}

			await stockTransaction.AddToDb();

			return new PutStockTransactionsResponse("success", stockTransaction.id!);
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			return new PutStockTransactionsResponse("error", "");
		}
	}
}