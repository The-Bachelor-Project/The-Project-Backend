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
				return Results.Ok(new PutStockTransactionsResponse("error", 0));
			}
			return Results.Ok(Endpoint(accessToken, body));
		});
	}

	public async static Task<PutStockTransactionsResponse> Endpoint(string accessToken, PutStockTransactionsBody body)
	{
		if (body.id is 0 || body.portfolio is null)
		{
			throw new StatusCodeException(400, "Missing stockTransaction id or portfolio id");
		}
		User user = new TokenSet(accessToken).GetUser();
		Portfolio portfolio = user.GetPortfolio(body.portfolio);
		if (body.newCurrency != "" && !(Tools.ValidCurrency.Check(body.newCurrency)))
		{
			throw new StatusCodeException(400, "Invalid currency " + body.newCurrency);
		}
		StockTransaction oldStockTransaction = portfolio.GetStockTransaction(body.id);
		StockTransaction newStockTransaction = new StockTransaction();
		newStockTransaction.portfolioId = oldStockTransaction.portfolioId;
		newStockTransaction.ticker = oldStockTransaction.ticker;
		newStockTransaction.exchange = oldStockTransaction.exchange;
		newStockTransaction.amount = oldStockTransaction.amount;
		newStockTransaction.timestamp = oldStockTransaction.timestamp;
		newStockTransaction.priceNative = oldStockTransaction.priceNative;
		newStockTransaction.priceUSD = oldStockTransaction.priceUSD;

		if (body.newAmount != 0)
		{
			newStockTransaction.amount = body.newAmount;
		}

		if (body.newPrice != 0 && body.newCurrency != "")
		{
			newStockTransaction.priceNative = new Money(body.newPrice, body.newCurrency);
		}
		else if (body.newPrice != 0)
		{
			newStockTransaction.priceNative = new Money(body.newPrice, newStockTransaction.priceNative!.currency);
		}
		else if (body.newCurrency != "")
		{
			newStockTransaction.priceNative = new Money(newStockTransaction.priceNative!.amount, body.newCurrency);
		}

		if (body.newTimestamp != 0)
		{
			newStockTransaction.timestamp = body.newTimestamp;
		}

		await newStockTransaction.AddToDb();
		await oldStockTransaction.Delete();

		System.Console.WriteLine("Old id " + oldStockTransaction.id + " new id " + newStockTransaction.id);

		return new PutStockTransactionsResponse("success", (int)newStockTransaction.id!);

	}
}