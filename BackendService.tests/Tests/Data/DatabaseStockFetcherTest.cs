namespace BackendService.tests;

[TestClass]
public class DatabaseStockFetcherTest
{
	private Data.Fetcher.DatabaseFetcher.StockFetcher stockFetcher = null!;
	[TestInitialize]
	public void Initialize()
	{
		stockFetcher = new Data.Fetcher.DatabaseFetcher.StockFetcher();
	}
	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetHistory_SuccessfulTest()
	{
		foreach (KeyValuePair<String, String> stock in Dictionaries.stockDictionary)
		{
			// Make sure it is in the database:
			Data.Fetcher.StockFetcher fetchFirst = new Data.Fetcher.StockFetcher();
			await fetchFirst.GetHistory(stock.Value, stock.Key, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD");

			Data.StockHistory stockHistory = await stockFetcher.GetHistory(stock.Value, stock.Key, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-01-20"), "daily", "USD");
			Assert.IsTrue(stockHistory != null, "Stock history should not be null");
			Assert.IsTrue(stockHistory.ticker == stock.Value, "Ticker should be " + stock.Value + " but was " + stockHistory.ticker);
			Assert.IsTrue(stockHistory.exchange == stock.Key, "Exchange should be " + stock.Key + " but was " + stockHistory.exchange);
			Assert.IsTrue(stockHistory.history.Count > 0, "History should not be empty for " + stock.Key + ":" + stock.Value);
		}
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetHistory_InvalidTickerTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("invalid", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetHistory_InvalidExchangeTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "invalid", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetHistory_InvalidCurrencyTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "invalid"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetHistory_InvalidDateTest()
	{
		String ticker = Dictionaries.stockDictionary.First().Value;
		String exchange = Dictionaries.stockDictionary.First().Key;
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory(ticker, exchange, DateOnly.Parse("2022-01-01"), DateOnly.Parse("2021-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetHistory_NullValuesTest()
	{
		// Ticker
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory(null!, "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Exchange
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", null!, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Interval
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), null!, "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Currency
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetProfile_SuccessfulTest()
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
	public async Task DatabaseStockFetcherTest_Search_SuccessfulTest()
	{
		Data.StockProfile[] searchResults = await stockFetcher.Search("AAPL");
		Assert.IsTrue(searchResults != null, "Search results should not be null");
		Assert.IsTrue(searchResults.Length > 0, "Search results should not be empty");
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_Search_TickerAndExhcangeTest()
	{
		Data.StockProfile[] searchResults = await stockFetcher.Search("CPH CHEMM");
		Assert.IsTrue(searchResults != null, "Search results should not be null");
		Assert.IsTrue(searchResults.Length == 1, "Search results should contain 1 result");
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_Search_NullQueryTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.Search(null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_Search_QueryNoResultsTest()
	{
		Data.StockProfile[] searchResults = await stockFetcher.Search("qwertyuiopasdfghjklzxcvbnm1234567890");
		Assert.IsTrue(searchResults != null, "Search results should not be null");
		Assert.IsTrue(searchResults.Length == 0, "Search results should be empty");
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetDividends_WithDividendsTest()
	{
		// Make sure it is in database
		Data.Fetcher.StockFetcher getStock = new Data.Fetcher.StockFetcher();
		await getStock.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD");

		List<Data.Dividend> dividends = await stockFetcher.GetDividends("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
		Assert.IsTrue(dividends.Count > 0, "Dividends should not be empty");
	}

	[TestMethod]
	public async Task DatabaseStockFetcherTest_GetDividends_WithoutDividendsTest()
	{
		List<Data.Dividend> dividends = await stockFetcher.GetDividends("TSLA", "NASDAQ", DateOnly.Parse("2021-02-01"), DateOnly.Parse("2022-01-01"));
		Assert.IsTrue(dividends.Count == 0, "Dividends should be empty");
	}
}