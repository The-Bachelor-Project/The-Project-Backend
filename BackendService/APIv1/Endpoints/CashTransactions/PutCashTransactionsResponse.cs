namespace API.v1;

public class PutCashTransactionsResponse
{
	public String response { get; set; }
	public int id { get; set; }

	public PutCashTransactionsResponse(String response, int id)
	{
		this.response = response;
		this.id = id;
	}
}
