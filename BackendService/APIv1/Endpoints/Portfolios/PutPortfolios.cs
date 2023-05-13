namespace API.v1;

using StockApp;

public class PutPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapPut("/v1/portfolios", (HttpContext context, PutPortfoliosBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new PutPortfoliosResponse("error"));
			}
			return Results.Ok(Endpoint(accessToken, body));
		});
	}

	public static PutPortfoliosResponse Endpoint(string accessToken, PutPortfoliosBody body)
	{
		if (body.id is null)
		{
			return new PutPortfoliosResponse("error");
		}
		try
		{
			User user = new TokenSet(accessToken).GetUser();
			Portfolio portfolio = user.GetPortfolio(body.id);
			if (body.newCurrency != "")
			{
				portfolio.ChangeCurrency(body.newCurrency);
			}
			if (body.newName != "")
			{
				portfolio.ChangeName(body.newName);
			}
			return new PutPortfoliosResponse("success");
		}
		catch (System.Exception)
		{
			return new PutPortfoliosResponse("error");
		}
	}
}