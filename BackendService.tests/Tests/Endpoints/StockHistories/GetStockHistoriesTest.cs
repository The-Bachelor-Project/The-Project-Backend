namespace BackendService.tests;

[TestClass]
public class GetStockHistoriesTest
{
	[TestMethod]
	public async Task GetStockHistoriesTest_GetWithoutDividendTest()
	{
		GetStockHistoriesResponse response = await GetStockHistories.Endpoint("TSLA", "NASDAQ", "2020-01-01", "2021-01-01", "daily");
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.history.history.Count > 0, "History should not be empty");
		Assert.IsTrue(response.history.history[0].closePrice.amount != 0, "Close should not be 0");
		Assert.IsTrue(response.history.history[0].openPrice.amount != 0, "Open should not be 0");
		Assert.IsTrue(response.history.history[0].highPrice.amount != 0, "High should not be 0");
		Assert.IsTrue(response.history.history[0].lowPrice.amount != 0, "Low should not be 0");
		Assert.IsTrue(response.history.dividends.Count == 0, "Dividends should be empty");
		Assert.IsTrue(response.history.ticker == "TSLA", "Ticker should be TSLA but was " + response.history.ticker);
		Assert.IsTrue(response.history.exchange == "NASDAQ", "Exchange should be NASDAQ but was " + response.history.exchange);
		Assert.IsTrue(response.history.interval == "daily", "Interval should be 1d but was " + response.history.interval);
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_GetWithDividendsTest()
	{
		GetStockHistoriesResponse response = await GetStockHistories.Endpoint("VICI", "NYSE", "2020-01-01", "2021-01-01", "daily");
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.history.history.Count > 0, "History should not be empty");
		Assert.IsTrue(response.history.history[0].closePrice.amount != 0, "Close should not be 0");
		Assert.IsTrue(response.history.history[0].openPrice.amount != 0, "Open should not be 0");
		Assert.IsTrue(response.history.history[0].highPrice.amount != 0, "High should not be 0");
		Assert.IsTrue(response.history.history[0].lowPrice.amount != 0, "Low should not be 0");
		Assert.IsTrue(response.history.dividends.Count > 0, "Dividends should not be empty");
		Assert.IsTrue(response.history.ticker == "VICI", "Ticker should be VICI but was " + response.history.ticker);
		Assert.IsTrue(response.history.exchange == "NYSE", "Exchange should be NYSE but was " + response.history.exchange);
		Assert.IsTrue(response.history.interval == "daily", "Interval should be 1d but was " + response.history.interval);
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_InvalidDatesTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockHistories.Endpoint("TSLA", "NASDAQ", "2021-01-01", "2020-01-01", "daily"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_InvalidStock()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockHistories.Endpoint("invalid", "NASDAQ", "2020-01-01", "2021-01-01", "daily"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_InvalidExchange()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockHistories.Endpoint("TSLA", "invalid", "2020-01-01", "2021-01-01", "daily"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}
}