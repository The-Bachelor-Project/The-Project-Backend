using StockApp;
using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetTokens
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/tokens", (HttpContext context) =>
		{
			String? refreshToken = context.Items["RefreshToken"] as String;
			if (refreshToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			int? familyID = (int?)context.Items["FamilyID"];
			if (familyID == null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(Endpoint(refreshToken, familyID.Value));
		});
	}

	public static TokensResponse Endpoint(string refreshToken, int familyID)
	{
		TokenSet newTokenSet = new TokenSet().SetRefreshToken(refreshToken).Refresh(familyID);
		return new TokensResponse("success", newTokenSet);
	}
}