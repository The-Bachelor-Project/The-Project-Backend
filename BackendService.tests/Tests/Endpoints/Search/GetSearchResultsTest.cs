namespace BackendService.tests;

[TestClass]
public class GetSearchResultsTest
{
	[TestMethod]
	public void GetSearchResultsTest_SuccessfulSearchTest()
	{
		GetSearchResultsResponse response = GetSearchResults.Endpoint("AAPL", true);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stocks.Length > 0, "Response should have at least one stock");
	}

	[TestMethod]
	public void GetSearchResultsTest_EmptyQueryTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetSearchResults.Endpoint("", true));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void GetSearchResultsTest_QueryNoResultsTest()
	{
		GetSearchResultsResponse response = GetSearchResults.Endpoint("qwertyuiopasdfghjklzxcvbnm1234567890", true);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stocks.Length == 0, "Response should have no stocks but had " + response.stocks.Length);
	}

	[TestMethod]
	public void GetSearchResultsTest_NullQueryTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetSearchResults.Endpoint(null!, true));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}
}