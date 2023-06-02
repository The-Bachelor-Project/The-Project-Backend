namespace API.v1;

public class PostUserPreferences
{
	// Make the user id part of the path, like "/v1/users/{id}/preferences"
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/users/preferences", (HttpContext context, PostUserPreferencesBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return new PostUserPreferencesResponse("error", -1);
			}
			return Endpoint(body, accessToken);
		});
	}

	public static PostUserPreferencesResponse Endpoint(PostUserPreferencesBody body, String accessToken)
	{
		if (body.setting is null || body.value is null || body.value == "" || body.setting == "" || accessToken == "" || accessToken is null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}

		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		int id = user.PostPreference(body.setting, body.value);
		return new PostUserPreferencesResponse("success", id);
	}
}