namespace BackendService.tests;

[TestClass]
public class CurrencyHistoriesTest
{
	[TestMethod]
	public async Task EndpointGetCurrencyHistoriesTest()
	{
		String currency = "CAD";
		API.v1.GetCurrencyHistoriesResponse response = await API.v1.GetCurrencyHistories.Endpoint(currency, new DateOnly(2021, 1, 1), new DateOnly(2022, 1, 2), Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.history.currency == currency, "currency should be " + currency + " but was " + response.history.currency);
		Assert.IsTrue(response.history.history[0].date < response.history.history[5].date, "Currency history is not sorted correctly");
		Assert.IsFalse(response.history.history.Count == 0, "Currency history is empty");
	}
}