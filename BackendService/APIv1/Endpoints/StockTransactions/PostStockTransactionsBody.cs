namespace API.v1;
public class PostStockTransactionsBody
{
	public StockApp.StockTransaction transaction { get; }


	public PostStockTransactionsBody(StockApp.StockTransaction transaction)
	{
		this.transaction = transaction;
	}
}