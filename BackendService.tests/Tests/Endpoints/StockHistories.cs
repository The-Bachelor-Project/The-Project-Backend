namespace BackendService.tests;

[TestClass]
public class StockHistoriesTest
{
	[TestMethod]
	public async Task EndpointGetStockHistories()
	{
		String ticker = "AAPL";
		String exchange = "NASDAQ";
		API.v1.GetStockHistoriesResponse response = await API.v1.GetStockHistories.Endpoint(ticker, exchange, "2021-01-01", "2022-01-01", "daily", Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.history.ticker == ticker, "ticker should be " + ticker + " but was " + response.history.ticker);
		Assert.IsTrue(response.history.exchange == exchange, "exchange should be " + exchange + " but was " + response.history.exchange);
		Assert.IsTrue(response.history.history[0].date < response.history.history[5].date, "Stock history is not sorted correctly");
		Assert.IsFalse(response.history.history.Count == 0, "Stock history is empty");
	}
}