namespace BackendService.tests;

using Data;

[TestClass]
public class YFStockSearchTest
{
	[TestMethod]
	public async Task YFSingleResultSearchTest()
	{
		Data.StockProfile[] result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().Search("tesla");
		Assert.IsFalse(result.Length == 0 || result.Length < 0, "Result is empty");
		Assert.IsTrue(result[0].ticker == "TSLA", "Result ticker on search, 'tesla' is not correct, should be TSLA but is " + result[0].ticker);
		Assert.IsTrue(result[0].name == "Tesla, Inc.", "Result name is not correct, should be Tesla, Inc. but is " + result[0].name);
	}

	[TestMethod]
	public async Task YFMultiResultSearchTest()
	{
		Data.StockProfile[] result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().Search("t");
		Assert.IsFalse(result.Length == 0 || result.Length < 0, "Result is empty");
		Assert.IsTrue(result[5].ticker == "TGT", "The ticker on the search result number 5 is not correct, should be TGT but is " + result[5].ticker);
	}
}