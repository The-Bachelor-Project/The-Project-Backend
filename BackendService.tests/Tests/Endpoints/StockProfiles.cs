namespace BackendService.tests;

[TestClass]
public class StockProfileTest
{
	[TestMethod]
	public async Task EndpointGetStockProfile()
	{
		String ticker = "SIA";
		String exchange = "TSE";
		API.v1.GetStockProfilesResponse response = await API.v1.GetStockProfiles.Endpoint(ticker, exchange);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "exchange should be " + exchange + " but was " + response.stock!.exchange);
		Assert.IsFalse(response.stock!.displayName == "", "name should not be empty");
	}
}