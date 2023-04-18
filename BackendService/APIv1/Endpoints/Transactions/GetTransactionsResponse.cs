namespace API.v1;

class GetTransactionsResponse
{
	public string response { get; }
	public List<BusinessLogic.Portfolio> portfolios { get; }
	public GetTransactionsResponse(string response, List<BusinessLogic.Portfolio> portfolios)
	{
		this.response = response;
		this.portfolios = portfolios;
	}
}