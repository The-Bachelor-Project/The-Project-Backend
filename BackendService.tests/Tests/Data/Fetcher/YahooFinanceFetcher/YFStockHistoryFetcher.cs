namespace BackendService.tests;

using Data;

[TestClass]
public class YFStockHistoryFetcherTest
{
	[TestMethod]
	public async Task YFNasdaqHistoryFetcherTest()
	{
		StockHistory result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory("goog", "nasdaq", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
		Assert.IsTrue(result != null);
		Assert.IsTrue(result.history.Count > 0);
		Assert.IsTrue(result.history[0].date < result.history[5].date);
	}

	[TestMethod]
	public async Task YFNyseHistoryFetcherTest()
	{
		StockHistory result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory("vici", "nyse", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
		Assert.IsTrue(result != null);
		Assert.IsTrue(result.history.Count > 0);
		Assert.IsTrue(result.history[0].date < result.history[5].date);
	}

	[TestMethod]
	public async Task YFCphFetcherTest()
	{
		StockHistory result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory("novo-b", "cph", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
		Assert.IsTrue(result != null);
		Assert.IsTrue(result.history.Count > 0);
		Assert.IsTrue(result.history[0].date < result.history[5].date);
	}

	[TestMethod]
	public async Task YFStoHistoryFetcherTest()
	{
		StockHistory result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory("VESTUM", "STO", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
		Assert.IsTrue(result != null);
		Assert.IsTrue(result.history.Count > 0);
		Assert.IsTrue(result.history[0].date < result.history[5].date);
	}

	[TestMethod]
	public async Task YFTseHistoryFetcherTest()
	{
		StockHistory result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory("SIA", "TSE", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
		Assert.IsTrue(result != null);
		Assert.IsTrue(result.history.Count > 0);
		Assert.IsTrue(result.history[0].date < result.history[5].date);
	}

	//TODO: Fix GBX on YahooFinance - only able to fetch GBP
	// [TestMethod]
	// public async Task YFLonHistoryFetcherTest()
	// {
	// 	StockHistory result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory("SHEL", "LON", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
	// 	Assert.IsTrue(result != null);
	// 	Assert.IsTrue(result.history.Count > 0);
	// 	Assert.IsTrue(result.history[0].date < Result.history[5].date);
	// }

	[TestMethod]
	public async Task YFHelHistoryFetcherTest()
	{
		StockHistory result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory("CGCBV", "HEL", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
		Assert.IsTrue(result != null);
		Assert.IsTrue(result.history.Count > 0);
		Assert.IsTrue(result.history[0].date < result.history[5].date);
	}
}