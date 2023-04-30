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
	}
}
