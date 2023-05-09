using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetSearchResults
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/search-results", ([FromQuery] string query, bool stocks) =>
		{
			return Results.Ok(Endpoint(query, stocks));
		});
	}

	public static GetSearchResultsResponse Endpoint(string query, bool stocks)
	{
		GetSearchResultsResponse results = new GetSearchResultsResponse("success");
		if (stocks)
		{
			results.stocks = new Data.Fetcher.StockFetcher().Search(query).Result;
		}
		return results;
	}

}
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
