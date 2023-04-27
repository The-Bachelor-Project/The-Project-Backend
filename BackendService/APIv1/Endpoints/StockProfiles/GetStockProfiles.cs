using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetStockProfiles
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-profiles", async ([FromQuery] string ticker, string exchange, string accessToken) =>
		{
			return Results.Ok(await Endpoint(ticker, exchange, accessToken));
		});
	}
	public static async Task<GetStockProfilesResponse> Endpoint(string ticker, string exchange, string accessToken)
	{
		return new GetStockProfilesResponse("success", await (new Data.Fetcher.StockFetcher().GetProfile(ticker, exchange)));
	}
}