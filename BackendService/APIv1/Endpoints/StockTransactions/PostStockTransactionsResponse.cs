namespace API.v1;

class PostStockTransactionsResponse
{
	public string response { get; }
	public string id { get; }
	public PostStockTransactionsResponse(string response, string id)
	{
		this.response = response;
		this.id = id;
	}
}