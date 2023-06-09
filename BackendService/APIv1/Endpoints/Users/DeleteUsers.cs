namespace API.v1;
using Microsoft.AspNetCore.Mvc;
public class DeleteUsers
{
	public static void Setup(WebApplication app)
	{
		app.MapDelete("/v1/users", (HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Endpoint(accessToken!);
		});
	}

	public static DeleteUsersResponse Endpoint(String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		user.Delete();
		return new DeleteUsersResponse("success");
	}
}