namespace BackendService;
class Search
{
	public static async Task<SearchResponse> endpoint(SearchBody body) //TODO: not done at all
	{
		SearchResponse searchResponse = new SearchResponse("error", "");

		await YfSearch.searchStocksAsync(body.term);

		// TODO: search hour own database here (after the yahoo search)
		//This function should return a JSON string with results.

		return searchResponse;
	}
}

class SearchResponse
{
	public SearchResponse(string response, string results)
	{
		this.response = response;
		this.results = results;
	}

	public String response { get; set; }
	public String results { get; set; }
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