namespace API.v1;

public class PutStockTransactionsResponse
{
	public string response;
	public int id;
	public String? error { get; set; }

	public PutStockTransactionsResponse(string response, int id)
	{
		this.response = response;
		this.id = id;
	}
}
