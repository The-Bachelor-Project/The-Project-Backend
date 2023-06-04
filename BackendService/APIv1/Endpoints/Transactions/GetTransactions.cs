using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/transactions", ([FromQuery] String currency, HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(Endpoint(accessToken, currency));
		});
	}

	public static GetTransactionsResponse Endpoint(String accessToken, String currency)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();

		return new GetTransactionsResponse("success", user.GetTransactions());
	}
}