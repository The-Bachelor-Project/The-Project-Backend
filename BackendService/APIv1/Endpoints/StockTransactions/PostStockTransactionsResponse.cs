namespace API.v1.Endpoints;

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