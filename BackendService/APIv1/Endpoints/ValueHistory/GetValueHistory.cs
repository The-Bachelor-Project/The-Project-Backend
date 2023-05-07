using Microsoft.AspNetCore.Mvc;

namespace API.v1;
class GetValueHistory
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/value-history", async (HttpContext context, [FromQuery] string startDate, string endDate) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(await (new StockApp.EndpointHandler.ValueHistory(accessToken)).Get("USD", DateOnly.Parse(startDate), DateOnly.Parse(endDate)));
		});
	}
}
