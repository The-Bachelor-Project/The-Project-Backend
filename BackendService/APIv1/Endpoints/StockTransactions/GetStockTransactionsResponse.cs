namespace API.v1.Endpoints;

class GetStockTransactionsResponse
{
	public string response { get; }
	public List<Data.StockTransaction> stockTransactions { get; }
	public GetStockTransactionsResponse(string response, List<Data.StockTransaction> stockTransactions)
	{
		this.response = response;
		this.stockTransactions = stockTransactions;
	}
}