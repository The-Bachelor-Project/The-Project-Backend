using Microsoft.AspNetCore.Mvc;

namespace API.v1;
class GetValueHistory
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/value-history", async ([FromQuery] string accessToken) =>
		{
			throw new NotImplementedException();
		});

		app.MapGet("/v1/value-history/{portfolio}", async ([FromQuery] string accessToken, string portfolio) =>
		{
			return Results.Ok(await Endpoint(accessToken, portfolio));
		});

		app.MapGet("/v1/value-history/{portfolio}/{exchange}/{ticker}", async ([FromQuery] string accessToken, string portfolio, string exchange, string ticker) =>
		{
			throw new NotImplementedException();
		});
	}

	public static async Task<GetStockHistoriesResponse> Endpoint(string accessToken, string portfolio)
	{
		throw new NotImplementedException();
	}
}
