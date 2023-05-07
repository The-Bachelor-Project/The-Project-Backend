namespace BackendService.tests;

[TestClass]
public class SearchResultsTest
{
	[TestMethod, Priority(1)]
	public void EndpointGetSingleSearchResult()
	{
		API.v1.GetSearchResultsResponse response = API.v1.GetSearchResults.Endpoint("AAPL", true);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.stocks.Length == 1, "response.stocks.Length should be 1 but was " + response.stocks.Length);
		Assert.IsTrue(response.stocks[0].displayName == "Apple Inc.", "response.stocks[0].name should be \"Apple Inc.\" but was " + response.stocks[0].displayName);
		Assert.IsTrue(response.stocks[0].ticker == "AAPL", "response.stocks[0].ticker should be \"AAPL\" but was " + response.stocks[0].ticker);
		Assert.IsTrue(response.stocks[0].exchange == "NASDAQ", "response.stocks[0].exchange should be \"NASDAQ\" but was " + response.stocks[0].exchange);
	}
}