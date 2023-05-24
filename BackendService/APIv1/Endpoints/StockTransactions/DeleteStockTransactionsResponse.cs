namespace API.v1;

public class DeleteStockTransactionsResponse
{
	public string response;
	public String? error { get; set; }

	public DeleteStockTransactionsResponse(string response)
	{
		this.response = response;
	}
}
