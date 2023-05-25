namespace API.v1;

public class PostStockTransactionsResponse
{
	public string response { get; }
	public int id { get; }
	public PostStockTransactionsResponse(string response, int id)
	{
		this.response = response;
		this.id = id;
	}
}