namespace BackendService.tests;
using Data;

[TestClass]
public class DatabaseStockFetcherTest
{
	[TestMethod]
	public async Task DatabaseStockHistoryFetcherTest()
	{
		Boolean isSaved = await FetcherHelper.SaveStockHistoryToDB("goog", "nasdaq");
		if (isSaved)
		{
			StockHistory result = await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetHistory("goog", "nasdaq", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
			Assert.IsTrue(result != null, "Stock history is null");
			Assert.IsTrue(result.history.Count > 0, "Stock history is empty");
			Assert.IsTrue(result.history[0].date < result.history[5].date, "Stock history is not sorted correctly");
		}
		else
		{
			Assert.Fail("Stock history was not saved to database from either YF or DB");
		}
	}

	public async Task DatabaseStockProfileFetcherTest()
	{
		Boolean isSaved = await FetcherHelper.SaveStockProfileToDB("goog", "nasdaq");
		if (isSaved)
		{
			StockProfile result = await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetProfile("goog", "nasdaq");
			Assert.IsTrue(result != null, "Stock profile is null");
			Assert.IsTrue(result.ticker == "goog", "Stock profile ticker is not correct, should be goog but is " + result.ticker);
		}
		else
		{
			Assert.Fail("Stock profile was not saved to database from either YF or DB");
		}
	}
}