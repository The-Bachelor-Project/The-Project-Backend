namespace BackendService.tests;

[TestClass]
public class SearchResultsTest
{
	[TestMethod]
	public void EndpointGetSingleSearchResult()
	{
		API.v1.GetSearchResultsResponse response = API.v1.GetSearchResults.Endpoint("AAPL", true, Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.stocks.Length == 1, "response.stocks.Length should be 1 but was " + response.stocks.Length);
		Assert.IsTrue(response.stocks[0].displayName == "Apple Inc.", "response.stocks[0].name should be \"Apple Inc.\" but was " + response.stocks[0].displayName);
		Assert.IsTrue(response.stocks[0].ticker == "AAPL", "response.stocks[0].ticker should be \"AAPL\" but was " + response.stocks[0].ticker);
		Assert.IsTrue(response.stocks[0].exchange == "NASDAQ", "response.stocks[0].exchange should be \"NASDAQ\" but was " + response.stocks[0].exchange);
	}

	[TestMethod]
	public void EndpointGetMultipleSearchResults()
	{
		API.v1.GetSearchResultsResponse response = API.v1.GetSearchResults.Endpoint("ma", true, Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.stocks.Length > 1, "response.stocks.Length should be more than 1 but was " + response.stocks.Length);
		Assert.IsTrue(response.stocks[1].ticker == "BRMK", "response.stocks[1].name should be \"BRMK\" but was " + response.stocks[1].ticker);
		Assert.IsTrue(response.stocks[1].exchange == "NYSE", "response.stocks[1].exchange should be \"NYSE\" but was " + response.stocks[1].exchange);
		Assert.IsTrue(response.stocks[3].ticker == "MAERSK-A", "response.stocks[0].name should be \"MAERSK-A\" but was " + response.stocks[3].ticker);
		Assert.IsTrue(response.stocks[3].exchange == "CPH", "response.stocks[0].exchange should be \"CPH\" but was " + response.stocks[3].exchange);
	}
}