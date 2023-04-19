using Microsoft.AspNetCore.Mvc;

namespace API.v1;

class GetSearchResults
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/search-results", ([FromQuery] string query, bool stocks, string accessToken) =>
		{
			return Results.Ok(Endpoint(query, stocks, accessToken));
		});
	}

	public static GetSearchResultsResponse Endpoint(string query, bool stocks, string accessToken)
	{
		GetSearchResultsResponse Results = new GetSearchResultsResponse("success");
		if (stocks)
		{
			Results.stocks = new Data.Fetcher.StockProfile().Search(query).Result;
		}
		return Results;
	}

}
class GetSearchResultsResponse
{
	public GetSearchResultsResponse(string response)
	{
		this.response = response;
		this.stocks = new Data.StockProfile[] { };
	}

	public String response { get; set; }
	public Data.StockProfile[] stocks { get; set; }
}
