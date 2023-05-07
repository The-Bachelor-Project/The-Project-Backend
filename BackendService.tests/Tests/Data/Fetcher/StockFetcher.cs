namespace BackendService.tests;
using Data;

[TestClass]
public class StockFetcherTest
{
	[TestMethod, Priority(1)]
	public async Task SaveStockHistoryToDBFromYFTest()
	{
		String ticker = "GNL";
		String exchange = "nyse";
		Boolean isReset = FetcherHelper.ResetHistory(ticker, exchange, 1);
		if (isReset)
		{
			StockHistory result = await new Data.Fetcher.StockFetcher().GetHistory(ticker, exchange, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
			Assert.IsTrue(result != null, "Result is null");
			Assert.IsTrue(result.history.Count > 0, "Result history is empty");
			Assert.IsTrue(result.history[0].date < result.history[5].date, "Result history is not sorted correctly");
		}
		else
		{
			Assert.Fail("Stock history was not resetted correctly");
		}
	}

	[TestMethod, Priority(1)]
	public async Task StockHistoryAlreadyInDBTest()
	{
		String ticker = "GNL";
		String exchange = "nyse";
		Boolean isSaved = await FetcherHelper.SaveStockHistoryToDB(ticker, exchange);
		if (isSaved)
		{
			StockHistory result = await new Data.Fetcher.StockFetcher().GetHistory(ticker, exchange, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
			Assert.IsTrue(result != null, "Result is null");
			Assert.IsTrue(result.history.Count > 0, "Result history is empty");
			Assert.IsTrue(result.history[0].date < result.history[5].date, "Result history is not sorted correctly");
		}
		else
		{
			Assert.Fail("Stock history was not added to DB correctly");
		}
	}

	[TestMethod, Priority(1)]
	public async Task SaveStockProfileToDBFromYFTest()
	{
		String ticker = "GNL";
		String exchange = "nyse";
		Boolean isDeleted = FetcherHelper.DeleteStockProfile(ticker, exchange);
		if (isDeleted)
		{
			StockProfile result = await new Data.Fetcher.StockFetcher().GetProfile(ticker, exchange);
			Assert.IsTrue(result != null, "Result is null");
			Assert.IsTrue(result.displayName == "Global Net Lease", "Result name is not correct, should be Global Net Lease but is " + result.displayName);
			Assert.IsTrue(result.ticker == "GNL", "Result ticker is not correct, should be GNL but is " + result.ticker);
		}
		else
		{
			Assert.Fail("Stock profile was not deleted correctly");
		}
	}

	[TestMethod, Priority(1)]
	public async Task StockProfileAlreadyInDBTest()
	{
		String ticker = "GNL";
		String exchange = "nyse";
		Boolean isSaved = await FetcherHelper.SaveStockProfileToDB(ticker, exchange);
		if (isSaved)
		{
			StockProfile result = await new Data.Fetcher.StockFetcher().GetProfile(ticker, exchange);
			Assert.IsTrue(result != null, "Result is null");
			Assert.IsTrue(result.displayName == "Global Net Lease", "Result name is not correct, should be Global Net Lease but is " + result.displayName);
			Assert.IsTrue(result.ticker == "GNL", "Result ticker is not correct, should be GNL but is " + result.ticker);
		}
		else
		{
			Assert.Fail("Stock profile was added to DB correctly");
		}
	}

}