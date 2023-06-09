namespace API.v1;
using Microsoft.AspNetCore.Mvc;

public class DeletePortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapDelete("/v1/portfolios", ([FromQuery] String id, HttpContext context) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Endpoint(id, accessToken!);
		});
	}

	public static DeletePortfoliosResponse Endpoint(String id, String accessToken)
	{
		StockApp.User user = new StockApp.TokenSet(accessToken).GetUser();
		user.GetPortfolios(id).Delete();
		return new DeletePortfoliosResponse("success");
	}
}