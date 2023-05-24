namespace API.v1;

public class GetStockTransactionsResponse
{
	public string response { get; }
	public List<Data.StockTransaction> stockTransactions { get; }
	public String? error { get; set; }
	public GetStockTransactionsResponse(string response, List<Data.StockTransaction> stockTransactions)
	{
		this.response = response;
		this.stockTransactions = stockTransactions;
	}
}