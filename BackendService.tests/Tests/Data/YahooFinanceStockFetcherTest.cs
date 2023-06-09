namespace BackendService.tests;

[TestClass]
public class YahooFinanceStockFetcherTest
{
	private Data.Fetcher.YahooFinanceFetcher.StockFetcher stockFetcher = null!;
	private Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher currencyFetcher = null!;
	[TestInitialize]
	public void Initialize()
	{
		stockFetcher = new Data.Fetcher.YahooFinanceFetcher.StockFetcher();
		currencyFetcher = new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher();
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetHistory_SuccessfulTest()
	{
		foreach (KeyValuePair<String, String> stock in Dictionaries.stockDictionary)
		{
			Data.StockHistory stockHistory = await stockFetcher.GetHistory(stock.Value, stock.Key, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD");
			Assert.IsTrue(stockHistory != null, "Stock history should not be null");
			Assert.IsTrue(stockHistory.ticker == stock.Value, "Ticker should be " + stock.Value + " but was " + stockHistory.ticker);
			Assert.IsTrue(stockHistory.exchange == stock.Key, "Exchange should be " + stock.Key + " but was " + stockHistory.exchange);
			Assert.IsTrue(stockHistory.history.Count > 0, "History should not be empty for " + stock.Key + ":" + stock.Value);
		}
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetHistory_InvalidTickerTest()
	{
		Data.StockHistory stockHistory = await stockFetcher.GetHistory("invalid", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD");
		Assert.IsTrue(stockHistory.history.Count == 0, "History should be empty for invalid ticker");
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetHistory_InvalidExchangeTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetHistory("AAPL", "invalid", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", "USD"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetHistory_EmptyHistoryTest()
	{
		Data.StockHistory stockHistory = await stockFetcher.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2021-02-01"), DateOnly.Parse("2021-01-01"), "daily", "USD");
		Assert.IsTrue(stockHistory.history.Count == 0, "History should be empty for invalid ticker");
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetHistory_AllCurrenciesTest()
	{
		foreach (String currency in Dictionaries.currencies)
		{
			Data.StockHistory stockHistory = await stockFetcher.GetHistory("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily", currency);
			Assert.IsTrue(stockHistory != null, "Stock history should not be null");
			Assert.IsTrue(stockHistory.ticker == "AAPL", "Ticker should be " + "AAPL" + " but was " + stockHistory.ticker);
			Assert.IsTrue(stockHistory.exchange == "NASDAQ", "Exchange should be " + "NASDAQ" + " but was " + stockHistory.exchange);
			Assert.IsTrue(stockHistory.history.Count > 0, "History should not be empty for " + "NASDAQ" + ":" + "AAPL");
		}
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetDividends_WithDividendsTest()
	{
		List<Data.Dividend> dividends = await stockFetcher.GetDividends("AAPL", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
		Assert.IsTrue(dividends.Count > 0, "Dividends should not be empty");
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetDividends_WithoutDividendsTest()
	{
		List<Data.Dividend> dividends = await stockFetcher.GetDividends("TSLA", "NASDAQ", DateOnly.Parse("2021-02-01"), DateOnly.Parse("2022-01-01"));
		Assert.IsTrue(dividends.Count == 0, "Dividends should be empty");
	}

	[TestMethod]
	public async Task YahooFinanceStockFetcherTest_GetDividends_InvalidTickerTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetDividends("invalid", "NASDAQ", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01")));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task YahooFinanceCurrencyFetcher_GetHistory_AllCurrenciesTest()
	{

		foreach (String currency in Dictionaries.currencies)
		{
			Data.CurrencyHistory currencyHistory = await currencyFetcher.GetHistory(currency, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
			Assert.IsTrue(currencyHistory != null, "Currency history should not be null");
			Assert.IsTrue(currencyHistory.currency == currency, "Currency should be " + currency + " but was " + currencyHistory.currency);
			Assert.IsTrue(currencyHistory.history.Count > 0, "History should not be empty for " + currency);
		}
	}

	[TestMethod]
	public async Task YahooFinanceCurrencyFetcher_GetHistory_EmptyHistoryTest()
	{
		Data.CurrencyHistory currencyHistory = await currencyFetcher.GetHistory("USD", DateOnly.Parse("2021-02-01"), DateOnly.Parse("2021-01-01"));
		Assert.IsTrue(currencyHistory.history.Count == 0, "History should be empty for invalid ticker");
	}

	// [TestMethod]
	// public async Task YahooFinanceStockFetcherTest_GetProfile_SuccessfulTest()
	// {
	// 	foreach (KeyValuePair<String, String> stock in Dictionaries.stockDictionary)
	// 	{
	// 		Data.StockProfile stockProfile = await stockFetcher.GetProfile(stock.Value, stock.Key);
	// 		Assert.IsTrue(stockProfile != null, "Stock profile should not be null");
	// 		Assert.IsTrue(stockProfile.ticker == stock.Value, "Ticker should be " + stock.Value + " but was " + stockProfile.ticker);
	// 		Assert.IsTrue(stockProfile.exchange == stock.Key, "Exchange should be " + stock.Key + " but was " + stockProfile.exchange);
	// 	}
	// }

	// [TestMethod]
	// public async Task YahooFinanceStockFetcherTest_GetProfile_InvalidTickerTest()
	// {
	// 	StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetProfile("invalid", "NASDAQ"));
	// 	Assert.IsTrue(exception.StatusCode == 500, "Status code should be 500 but was " + exception.StatusCode);
	// }

	// [TestMethod]
	// public async Task YahooFinanceStockFetcherTest_GetProfile_InvalidExchangeTest()
	// {
	// 	StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockFetcher.GetProfile("AAPL", "invalid"));
	// 	Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	// }
}