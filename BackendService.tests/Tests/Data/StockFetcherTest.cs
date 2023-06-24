namespace BackendService.tests;

[TestClass]
public class StockFetcherTest
{
	private Data.Fetcher.StockFetcher stockFetcher = null!;
	[TestInitialize]
	public void Initialize()
	{
		stockFetcher = new Data.Fetcher.StockFetcher();
	}
	[TestMethod]
	public async Task StockFetcherTest_GetHistory_SuccessfulTest()
	{
		foreach (KeyValuePair<String, String> stock in Dictionaries.stockDictionary)
		{
			Data.StockHistory stockHistory = await stockFetcher.GetHistory(stock.Value, stock.Key, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-01-20"), "daily", "USD");
			Assert.IsTrue(stockHistory != null, "Stock history should not be null");
			Assert.IsTrue(stockHistory.ticker == stock.Value, "Ticker should be " + stock.Value + " but was " + stockHistory.ticker);
			Assert.IsTrue(stockHistory.exchange == stock.Key, "Exchange should be " + stock.Key + " but was " + stockHistory.exchange);
			Assert.IsTrue(stockHistory.history.Count > 0, "History should not be empty for " + stock.Key + ":" + stock.Value);
		}
	}

	[TestMethod]
	public async Task StockFetcherTest_GetHistory_CurrenciesSuccessfulTest()
	{
		foreach (String currency in Dictionaries.currencies)
		{
			Data.StockHistory stockHistory = await stockFetcher.GetHistory("CHEMM", "CPH", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-01-20"), "daily", currency);
			Assert.IsTrue(stockHistory != null, "Stock history should not be null");
			Assert.IsTrue(stockHistory.ticker == "CHEMM", "Ticker should be CHEMM but was " + stockHistory.ticker);
			Assert.IsTrue(stockHistory.exchange == "CPH", "Exchange should be CPH but was " + stockHistory.exchange);
			Assert.IsTrue(stockHistory.history.Count > 0, "History should not be empty for CURRENCY:" + currency);
			Assert.IsTrue(stockHistory.history[0].highPrice.currency == currency, "Currency should be " + currency + " for high price but was " + stockHistory.history[0].highPrice.currency);
			Assert.IsTrue(stockHistory.history[0].lowPrice.currency == currency, "Currency should be " + currency + " for low price but was " + stockHistory.history[0].lowPrice.currency);
			Assert.IsTrue(stockHistory.history[0].openPrice.currency == currency, "Currency should be " + currency + " for open price but was " + stockHistory.history[0].openPrice.currency);
			Assert.IsTrue(stockHistory.history[0].closePrice.currency == currency, "Currency should be " + currency + " for close price but was " + stockHistory.history[0].closePrice.currency);
		}
	}

	[TestMethod]
	public async Task StockFetcherTest_GetHistory_InvalidCurrency()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("CHEMM", "CPH", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-01-20"), "daily", "invalid"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_GetHistory_InvalidTickerTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("invalid", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_GetHistory_InvalidExchangeTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "invalid", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_GetHistory_InvalidDateTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2022-01-01"), DateOnly.Parse("2021-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_GetHistory_NullValuesTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory(null!, "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", null!, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), null!, "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_GetProfile_SuccessfulTest()
	{
		foreach (KeyValuePair<String, String> stock in Dictionaries.stockDictionary)
		{
			Data.StockProfile stockProfile = await stockFetcher.GetProfile(stock.Value, stock.Key);
			Assert.IsTrue(stockProfile != null, "Stock profile should not be null");
			Assert.IsTrue(stockProfile.ticker == stock.Value, "Ticker should be " + stock.Value + " but was " + stockProfile.ticker);
			Assert.IsTrue(stockProfile.exchange == stock.Key, "Exchange should be " + stock.Key + " but was " + stockProfile.exchange);
		}
	}

	[TestMethod]
	public async Task StockFetcherTest_GetProfile_InvalidTickerTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetProfile("invalid", "NASDAQ"));
		Assert.IsTrue(exception.StatusCode == 500, "Status code should be 500 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_GetProfile_InvalidExchangeTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetProfile("AAPL", "invalid"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_GetProfile_NullValuesTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetProfile(null!, "NASDAQ"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetProfile("AAPL", null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_Search_SuccessfulTest()
	{
		Data.StockProfile[] searchResults = await stockFetcher.Search("AAPL");
		Assert.IsTrue(searchResults != null, "Search results should not be null");
		Assert.IsTrue(searchResults.Length > 0, "Search results should not be empty");
	}

	[TestMethod]
	public async Task StockFetcherTest_Search_EmptyQueryTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.Search(""));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_Search_NullQueryTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.Search(null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockFetcherTest_Search_QueryNoResultsTest()
	{
		Data.StockProfile[] searchResults = await stockFetcher.Search("qwertyuiopasdfghjklzxcvbnm1234567890");
		Assert.IsTrue(searchResults != null, "Search results should not be null");
		Assert.IsTrue(searchResults.Length == 0, "Search results should be empty");
	}

	[TestMethod]
	public async Task StockFetcherTest_GenerateTags_SuccessfulTest()
	{
		Data.StockProfile stockProfile = await stockFetcher.GetProfile("AAPL", "NASDAQ");
		String tags = stockFetcher.GenerateTags(stockProfile);
		Assert.IsTrue(tags != null, "Tags should not be null");
		Assert.IsTrue(tags.Length > 0, "Tags should not be empty");
		Assert.IsTrue(tags.Contains("aapl"), "Tags should contain AAPL but was " + tags);
	}
}