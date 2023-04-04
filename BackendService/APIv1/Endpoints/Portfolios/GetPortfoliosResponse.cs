namespace API.v1.Endpoints;
class GetPortfoliosResponse
{
	public GetPortfoliosResponse(string response, Data.Portfolio portfolio)
	{
		this.response = response;
		this.portfolio = portfolio;
	}

	public String response { get; set; }
	public Data.Portfolio portfolio { get; set; }
}