namespace API.v1;

public class GetStockTransactionsResponse
{
	public string response { get; }
	public List<StockApp.StockTransaction> stockTransactions { get; }
	public GetStockTransactionsResponse(string response, List<StockApp.StockTransaction> stockTransactions)
	{
		this.response = response;
		this.stockTransactions = stockTransactions;
	}
}