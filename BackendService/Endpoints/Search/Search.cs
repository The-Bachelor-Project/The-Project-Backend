namespace BackendService;
class Search
{
	public static async Task<SearchResponse> endpoint(SearchBody body) //TODO: not done at all
	{
		SearchResponse searchResponse = new SearchResponse("error");

		await YfSearch.searchStocksAsync(body.term);

		return searchResponse;
	}
}

class SearchResponse
{
	public SearchResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class SearchBody
{
	public SearchBody(string term, bool stocks)
	{
		this.term = term;
		this.stocks = stocks;
	}

	public String term { get; set; }
	public bool stocks { get; set; }
}