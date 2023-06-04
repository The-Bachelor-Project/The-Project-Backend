namespace API.v1;

public class GetTransactionsResponse
{
	public string response { get; }
	public List<Data.Transaction> transactions { get; }
	public GetTransactionsResponse(string response, List<Data.Transaction> transactions)
	{
		this.response = response;
		this.transactions = transactions;
	}
}