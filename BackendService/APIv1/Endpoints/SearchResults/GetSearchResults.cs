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
		if (query == null)
		{
			throw new StatusCodeException(400, "Required fields are missing");
		}
		if (query.Replace(" ", "").Length == 0)
		{
			throw new StatusCodeException(400, "Query cannot be empty");
		}
		GetSearchResultsResponse results = new GetSearchResultsResponse("success");
		if (stocks)
		{
			results.stocks = new Data.Fetcher.StockFetcher().Search(query).Result;
		}
		return results;
	}

}

