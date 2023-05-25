namespace API.v1;

public class GetSearchResultsResponse
{
	public GetSearchResultsResponse(string response)
	{
		this.response = response;
		this.stocks = new Data.StockProfile[] { };
	}

	public String response { get; set; }
	public Data.StockProfile[] stocks { get; set; }
}