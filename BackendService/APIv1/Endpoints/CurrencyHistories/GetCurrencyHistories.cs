using Microsoft.AspNetCore.Mvc;
namespace API.v1;

class GetCurrencyHistories
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/currency-histories", async ([FromQuery] string currency, DateOnly startDate, DateOnly endDate, string accessToken) =>
		{
			return Results.Ok(await Endpoint(currency, startDate, endDate, accessToken));
		});
	}

	private static async Task<GetCurrencyHistoriesResponse> Endpoint(string currency, DateOnly startDate, DateOnly endDate, string accessToken)
	{
		Data.CurrencyHistory result = await (new Data.Fetcher.CurrencyFetcher()).GetHistory(currency, startDate, endDate);
		return new GetCurrencyHistoriesResponse("success", result);
	}
}