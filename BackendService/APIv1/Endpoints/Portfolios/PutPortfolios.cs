namespace API.v1;

using StockApp;

public class PutPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapPut("/v1/portfolios/name", (HttpContext context, PutPortfoliosBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new PutPortfoliosResponse("error"));
			}
			return Results.Ok(EndpointName(accessToken, body));
		});

		app.MapPut("/v1/portfolios/currency", (HttpContext context, PutPortfoliosBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new PutPortfoliosResponse("error"));
			}
			return Results.Ok(EndpointCurrency(accessToken, body));
		});
	}

	public static PutPortfoliosResponse EndpointName(string accessToken, PutPortfoliosBody body)
	{
		if (body.changeString is null || body.id is null)
		{
			return new PutPortfoliosResponse("error");
		}
		try
		{
			User user = new TokenSet(accessToken).GetUser();
			Portfolio portfolio = user.GetPortfolio(body.id);
			portfolio.ChangeName(body.changeString);
			return new PutPortfoliosResponse("success");
		}
		catch (System.Exception)
		{
			return new PutPortfoliosResponse("error");
		}
	}

	public static PutPortfoliosResponse EndpointCurrency(string accessToken, PutPortfoliosBody body)
	{
		if (body.changeString is null || body.id is null)
		{
			System.Console.WriteLine("1");
			return new PutPortfoliosResponse("error");
		}
		try
		{
			User user = new TokenSet(accessToken).GetUser();
			Portfolio portfolio = user.GetPortfolio(body.id);
			portfolio.ChangeCurrency(body.changeString);
			return new PutPortfoliosResponse("success");
		}
		catch (System.Exception)
		{
			return new PutPortfoliosResponse("error");
		}
	}
}