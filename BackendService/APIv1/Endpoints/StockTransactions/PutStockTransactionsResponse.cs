namespace API.v1;

public class PutStockTransactionsResponse
{
	public string response;
	public string id;

	public PutStockTransactionsResponse(string response, string id)
	{
		this.response = response;
		this.id = id;
	}
}
