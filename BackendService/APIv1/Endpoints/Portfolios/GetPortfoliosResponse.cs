namespace API.v1;
class GetPortfoliosResponse
{
	public GetPortfoliosResponse(string response, List<StockApp.Portfolio> portfolios)
	{
		this.response = response;
		this.portfolios = portfolios;
	}

	public String response { get; set; }
	public List<StockApp.Portfolio> portfolios { get; set; }
}