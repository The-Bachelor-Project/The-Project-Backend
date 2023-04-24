namespace BackendService.tests;

using Data;

[TestClass]
public class YFStockSearchTest
{
	[TestMethod]
	public async Task YFSingleResultSearchTest()
	{
		Data.StockProfile[] result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().Search("tesla");
		Assert.IsFalse(result.Length == 0 || result.Length < 0);
		Assert.IsTrue(result[0].ticker == "TSLA");
		Assert.IsTrue(result[0].name == "Tesla, Inc.");
	}

	[TestMethod]
	public async Task YFMultiResultSearchTest()
	{
		Data.StockProfile[] result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().Search("t");
		Assert.IsFalse(result.Length == 0 || result.Length < 0);
		Assert.IsTrue(result[5].ticker == "TGT");
	}
}