namespace API.v1;

public class GetUserPreferences
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/users/preferences", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return new GetUserPreferencesResponse("error", null!);
			}
			return Endpoint(accessToken);
		});
	}

	public static GetUserPreferencesResponse Endpoint(String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		Dictionary<String, String> preferences = user.GetPreferences();
		return new GetUserPreferencesResponse("success", preferences);
	}
}