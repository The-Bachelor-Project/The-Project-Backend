namespace API.v1;

public class GetUserPreferences
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/users/preferences", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Endpoint(accessToken!);
		});
	}

	public static GetUserPreferencesResponse Endpoint(String accessToken)
	{
		if (accessToken == null || accessToken == "")
		{
			throw new StatusCodeException(401, "Missing access token");
		}
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		Dictionary<String, String> preferences = user.GetPreferences();
		return new GetUserPreferencesResponse("success", preferences);
	}
}