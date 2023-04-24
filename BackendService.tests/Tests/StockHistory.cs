// using Data;

// namespace BackendService.tests;

// [TestClass]
// public class StockHistoryTest
// {
// 	private String ticker = "CGCBV";
// 	private String exchange = "HEL";
// 	[TestMethod]
// 	public async Task YahooTest()
// 	{
// 		FetcherHelper.ResetStock(ticker, exchange);
// 		Data.Fetcher.YahooFinanceFetcher.StockFetcher fetcher = new();
// 		StockHistory Result = await fetcher.GetHistory(ticker, exchange, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
// 		Assert.IsTrue(Result != null);
// 		Assert.IsTrue(Result.history.Count > 0);
// 		Assert.IsTrue(Result.history[0].date < Result.history[5].date);
// 	}
// }