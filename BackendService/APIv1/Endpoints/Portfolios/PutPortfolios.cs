namespace API.v1;

using StockApp;

public class PutPortfolios
{
	public static void Setup(WebApplication app)
	{
		app.MapPut("/v1/portfolios", (HttpContext context, PutPortfoliosBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(Endpoint(accessToken!, body));
		});
	}

	public static PutPortfoliosResponse Endpoint(string accessToken, PutPortfoliosBody body)
	{
		if (body.id is null)
		{
			throw new StatusCodeException(400, "Required fields missing");
		}
		User user = new TokenSet(accessToken).GetUser();
		Portfolio portfolio = user.GetPortfolios(body.id);
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
}