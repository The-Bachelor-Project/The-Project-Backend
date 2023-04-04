namespace API.v1.Endpoints;

class PostPortfoliosBody
{
	public Data.Portfolio portfolio { get; }
	public string accessToken { get; }

	public PostPortfoliosBody(Data.Portfolio portfolio, string accessToken)
	{
		this.portfolio = portfolio;
		this.accessToken = accessToken;
	}
}