namespace API.v1;
class GetPortfoliosResponse
{
	public GetPortfoliosResponse(string response, List<BusinessLogic.Portfolio> portfolios)
	{
		this.response = response;
		this.portfolios = portfolios;
	}

	public String response { get; set; }
	public List<BusinessLogic.Portfolio> portfolios { get; set; }
}