namespace API.v1;
using Microsoft.AspNetCore.Mvc;
public class DeleteUsers
{
	public static void Setup(WebApplication app)
	{
		app.MapDelete("/v1/users", ([FromQuery] String email, HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return new DeleteUsersResponse("error");
			}
			return Endpoint(email, accessToken);
		});
	}

	public static DeleteUsersResponse Endpoint(String email, String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		if (user.Delete(email))
		{
			return new DeleteUsersResponse("success");
		}
		else
		{
			return new DeleteUsersResponse("error");
		}
	}
}