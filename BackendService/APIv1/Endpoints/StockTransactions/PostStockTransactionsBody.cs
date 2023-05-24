namespace API.v1;
public class PostStockTransactionsBody
{
	public Data.StockTransaction transaction { get; }
	public string accessToken { get; }


	public PostStockTransactionsBody(Data.StockTransaction transaction, string accessToken)
	{
		this.transaction = transaction;
		this.accessToken = accessToken;
	}
}