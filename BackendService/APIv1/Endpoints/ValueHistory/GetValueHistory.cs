using Microsoft.AspNetCore.Mvc;

namespace API.v1;
class GetValueHistory
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/value-history", async ([FromQuery] string accessToken, string startDate, string endDate) =>
		{
			return Results.Ok(await (new StockApp.EndpointHandler.ValueHistory(accessToken)).Get("USD", DateOnly.Parse(startDate), DateOnly.Parse(endDate)));
		});

		app.MapGet("/v1/value-history/{portfolio}", async ([FromQuery] string accessToken, string portfolio) =>
		{
			throw new NotImplementedException();
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
