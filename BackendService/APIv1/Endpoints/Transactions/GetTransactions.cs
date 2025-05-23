using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetTransactions
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/transactions", async (HttpContext context, [FromQuery] String currency) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(await Endpoint(accessToken!, currency));
		});
	}

	public async static Task<GetTransactionsResponse> Endpoint(String accessToken, String currency)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		return new GetTransactionsResponse("success", await user.GetTransactions(currency));
	}
}