namespace API.v1;

class GetTransactionsResponse
{
	public string response { get; }
	public List<StockApp.Portfolio> portfolios { get; }
	public String? error { get; set; }
	public GetTransactionsResponse(string response, List<StockApp.Portfolio> portfolios)
	{
		this.response = response;
		this.portfolios = portfolios;
	}
}