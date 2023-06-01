namespace BackendService.tests;

[TestClass]
public class GetStockHistoriesTest
{
	[TestMethod]
	public async Task GetStockHistoriesTest_GetWithoutDividendTest()
	{
		GetStockHistoriesResponse response = await GetStockHistories.Endpoint("TSLA", "NASDAQ", "2020-01-01", "2021-01-01", "daily", "USD");
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
		GetStockHistoriesResponse response = await GetStockHistories.Endpoint("VICI", "NYSE", "2020-01-01", "2021-01-01", "daily", "USD");
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
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockHistories.Endpoint("TSLA", "NASDAQ", "2021-01-01", "2020-01-01", "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_InvalidStock()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockHistories.Endpoint("invalid", "NASDAQ", "2020-01-01", "2021-01-01", "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_InvalidExchange()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockHistories.Endpoint("TSLA", "invalid", "2020-01-01", "2021-01-01", "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_CurrenciesConversionTest()
	{
		GetStockHistoriesResponse originalResponse = await GetStockHistories.Endpoint("CHEMM", "CPH", "2020-01-01", "2021-01-01", "daily", "USD");
		Assert.IsTrue(originalResponse.response == "success", "Response should be success but was " + originalResponse.response);
		Assert.IsTrue(originalResponse.history.history.Count > 0, "History should not be empty");
		Assert.IsTrue(originalResponse.history.history[0].closePrice.amount != 0, "Close should not be 0");
		Assert.IsTrue(originalResponse.history.history[0].openPrice.amount != 0, "Open should not be 0");
		Assert.IsTrue(originalResponse.history.history[0].highPrice.amount != 0, "High should not be 0");
		Assert.IsTrue(originalResponse.history.history[0].lowPrice.amount != 0, "Low should not be 0");
		Assert.IsTrue(originalResponse.history.dividends.Count > 0, "Dividends should not be empty");
		Assert.IsTrue(originalResponse.history.ticker == "CHEMM", "Ticker should be VICI but was " + originalResponse.history.ticker);
		Assert.IsTrue(originalResponse.history.exchange == "CPH", "Exchange should be NYSE but was " + originalResponse.history.exchange);

		foreach (String currency in Dictionaries.currencies)
		{
			GetStockHistoriesResponse convertedResponse = await GetStockHistories.Endpoint("CHEMM", "CPH", "2020-01-01", "2021-01-01", "daily", currency);
			Assert.IsTrue(convertedResponse.response == "success", "Response should be success but was " + convertedResponse.response);
			Assert.IsTrue(convertedResponse.history.history.Count > 0, "History should not be empty");
			Assert.IsTrue(convertedResponse.history.history[0].closePrice.amount != 0, "Close should not be 0");
			Assert.IsTrue(convertedResponse.history.history[0].openPrice.amount != 0, "Open should not be 0");
			Assert.IsTrue(convertedResponse.history.history[0].highPrice.amount != 0, "High should not be 0");
			Assert.IsTrue(convertedResponse.history.history[0].lowPrice.amount != 0, "Low should not be 0");
			Assert.IsTrue(convertedResponse.history.dividends.Count > 0, "Dividends should not be empty");
			Assert.IsTrue(convertedResponse.history.ticker == "CHEMM", "Ticker should be VICI but was " + convertedResponse.history.ticker);
			Assert.IsTrue(convertedResponse.history.exchange == "CPH", "Exchange should be NYSE but was " + convertedResponse.history.exchange);

			Assert.IsTrue(convertedResponse.history.history[0].closePrice.currency == currency, "Currency should be " + currency + " but was " + convertedResponse.history.history[0].closePrice.currency);
			Assert.IsTrue(convertedResponse.history.history[0].openPrice.currency == currency, "Currency should be " + currency + " but was " + convertedResponse.history.history[0].openPrice.currency);
			Assert.IsTrue(convertedResponse.history.history[0].highPrice.currency == currency, "Currency should be " + currency + " but was " + convertedResponse.history.history[0].highPrice.currency);
			Assert.IsTrue(convertedResponse.history.history[0].lowPrice.currency == currency, "Currency should be " + currency + " but was " + convertedResponse.history.history[0].lowPrice.currency);
			Assert.IsTrue(convertedResponse.history.history[0].lowPrice.amount != originalResponse.history.history[0].lowPrice.amount, "Amount of " + currency + " should not be the same as USD");
			Assert.IsTrue(convertedResponse.history.history[0].highPrice.amount != originalResponse.history.history[0].highPrice.amount, "Amount of " + currency + " should not be the same as USD");
			Assert.IsTrue(convertedResponse.history.history[0].openPrice.amount != originalResponse.history.history[0].openPrice.amount, "Amount of " + currency + " should not be the same as USD");
			Assert.IsTrue(convertedResponse.history.history[0].closePrice.amount != originalResponse.history.history[0].closePrice.amount, "Amount of " + currency + " should not be the same as USD");

			Assert.IsTrue(convertedResponse.history.dividends[0].payout.currency == currency, "Currency should be " + currency + " but was " + convertedResponse.history.dividends[0].payout.currency);
			Assert.IsTrue(convertedResponse.history.dividends[0].payout.amount != originalResponse.history.dividends[0].payout.amount, "Amount of " + currency + " should not be the same as USD");
		}
	}

	[TestMethod]
	public async Task GetStockHistoriesTest_InvalidCurrencyTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockHistories.Endpoint("CHEMM", "CPH", "2020-01-01", "2021-01-01", "daily", "invalid"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}
}