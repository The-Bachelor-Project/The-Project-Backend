namespace API.v1;

public class PostCashTransactionsResponse
{
	public String response { get; set; }
	public int id { get; set; }

	public PostCashTransactionsResponse(String response, int id)
	{
		this.response = response;
		this.id = id;
	}
}
