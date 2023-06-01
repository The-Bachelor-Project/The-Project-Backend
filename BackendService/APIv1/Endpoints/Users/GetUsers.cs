

namespace API.v1;

public class GetUsers
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/users", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new GetPortfoliosResponse("error", new List<StockApp.Portfolio> { }));
			}
			return Results.Ok(Endpoint(accessToken));
		});
	}

	public static GetUsersResponse Endpoint(string accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		return new GetUsersResponse("success", user.id!);
	}
}
