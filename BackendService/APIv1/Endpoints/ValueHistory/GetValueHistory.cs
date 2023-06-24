using Microsoft.AspNetCore.Mvc;

namespace API.v1;
public class GetValueHistory
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/value-history", async (HttpContext context, [FromQuery] string startDate, string endDate, string currency) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(await EndpointAsync(startDate, endDate, currency, accessToken!));
		});
	}

	public static async Task<GetValueHistoryResponse> EndpointAsync(string startDate, string endDate, string currency, string accessToken)
	{
		GetValueHistoryResponse response = new GetValueHistoryResponse(await (new StockApp.EndpointHandler.ValueHistory(accessToken)).Get(currency, DateOnly.Parse(startDate), DateOnly.Parse(endDate)));
		return response;
	}
}
