using Microsoft.AspNetCore.Mvc;
namespace API.v1;

public class GetCurrencyHistories
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/currency-histories", async (HttpContext context, [FromQuery] string currency, DateOnly startDate, DateOnly endDate) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(await Endpoint(currency, startDate, endDate, accessToken));
		});
	}

	public static async Task<GetCurrencyHistoriesResponse> Endpoint(string currency, DateOnly startDate, DateOnly endDate, string accessToken)
	{
		Data.CurrencyHistory result = await (new Data.Fetcher.CurrencyFetcher()).GetHistory(currency, startDate, endDate);
		return new GetCurrencyHistoriesResponse("success", result);
	}
}