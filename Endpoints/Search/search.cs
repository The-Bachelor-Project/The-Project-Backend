class Search
{
    public static async Task<SearchResponse> endpoint(SearchBody body)
    {
        SearchResponse searchResponse = new SearchResponse();
        
		await YfSearch.searchStocksAsync(body.term);
        
        return searchResponse;
    }
}

class SearchResponse
{
    public String response { get; set; }
}

class SearchBody
{
    public String term { get; set; }
    public bool stocks { get; set; }
}