namespace API.v1;

public class GetTransactionsResponse
{
	public string response { get; }
	public List<StockApp.Portfolio> portfolios { get; }
	public GetTransactionsResponse(string response, List<StockApp.Portfolio> portfolios)
	{
		this.response = response;
		this.portfolios = portfolios;
	}
}