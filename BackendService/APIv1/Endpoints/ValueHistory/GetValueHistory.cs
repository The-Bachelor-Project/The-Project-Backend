using Microsoft.AspNetCore.Mvc;

namespace API.v1;
class GetValueHistory
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/value-history", async ([FromQuery] string accessToken) =>
		{
			return Results.Ok(await Endpoint(accessToken));
		});

		app.MapGet("/v1/value-history/{portfolio}", async ([FromQuery] string accessToken, string portfolio) =>
		{
			return Results.Ok(portfolio);
		});

		app.MapGet("/v1/value-history/{portfolio}/{exchange}/{ticker}", async ([FromQuery] string accessToken, string portfolio, string exchange, string ticker) =>
		{
			return Results.Ok(await Endpoint(accessToken));
		});
	}

	public static async Task<GetStockHistoriesResponse> Endpoint(string accessToken)
	{
		throw new NotImplementedException();
	}
}
