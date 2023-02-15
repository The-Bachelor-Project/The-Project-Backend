using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BackendService;
class Search
{
	public static async Task<SearchResponse> endpoint(SearchBody body) //TODO: not done at all
	{
		try
		{
			SearchResponse searchResponse = new SearchResponse("error");
			searchResponse.response = "success";
			searchResponse.stocks = await DatabaseService.Search.Stocks(body.term);
			return searchResponse;
		}
		catch (Exception e)
		{
			throw e;
		}
	}
}

class SearchResponse
{
	public SearchResponse(string response)
	{
		this.response = response;
		this.stocks = new Data.StockProfile[] { };
	}

	public String response { get; set; }
	public Data.StockProfile[] stocks { get; set; }
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