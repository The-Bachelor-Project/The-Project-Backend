using Microsoft.AspNetCore.Mvc;

namespace API.v1.Endpoints;

class GetStockProfiles
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-profiles", ([FromQuery] string ticker, string exchange, string accessToken) =>
		{
			return Results.Ok(Endpoint(ticker, exchange, accessToken));
		});
	}
	public static GetStockProfilesResponse Endpoint(string ticker, string exchange, string accessToken)
	{
		return new GetStockProfilesResponse("success", new Data.StockProfile(ticker, exchange, "name"));
	}
}