namespace API.v1;
using Microsoft.AspNetCore.Mvc;

public class DeleteUserPreferences
{
	public static void Setup(WebApplication app)
	{
		app.MapDelete("/v1/users/preferences", ([FromQuery] String setting, HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Endpoint(setting, accessToken!);
		});
	}

	public static DeleteUserPreferencesResponse Endpoint(String setting, String accessToken)
	{
		if (setting == null || setting == "" || accessToken == null || accessToken == "")
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		user.DeletePreference(setting);
		return new DeleteUserPreferencesResponse("success");
	}
}