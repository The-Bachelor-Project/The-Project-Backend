namespace API.v1.Endpoints;
class PostStockTransactionsBody
{
	public Data.StockTransaction stockTransaction { get; }
	public string accessToken { get; }

	public PostStockTransactionsBody(Data.StockTransaction stockTransaction, string accessToken)
	{
		this.stockTransaction = stockTransaction;
		this.accessToken = accessToken;
	}
}