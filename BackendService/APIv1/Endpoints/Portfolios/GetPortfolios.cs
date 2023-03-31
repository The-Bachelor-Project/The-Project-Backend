using Microsoft.AspNetCore.Mvc;

namespace API.v1;

class GetPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/portfolios", ([FromQuery] string id, string accessToken) =>
		{
			return Results.Ok(Endpoint(id, accessToken));
		});
	}

	public static GetPortfoliosResponse Endpoint(string id, string accessToken)
	{
		return new GetPortfoliosResponse("success", new Data.Portfolio("name", id, "currency", (decimal)0.0, true));
	}
}