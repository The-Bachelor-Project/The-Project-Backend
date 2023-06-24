namespace API.v1;

public class DeleteCashTransactionsResponse
{
	public string response { get; set; }

	public DeleteCashTransactionsResponse(string response)
	{
		this.response = response;
	}
}
