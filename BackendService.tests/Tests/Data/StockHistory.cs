using Data;

namespace BackendService.tests;

[TestClass]
public class StockProfileTest
{
	[TestMethod]
	public async Task FetcherTest()
	{
		StockProfile Result = await new Data.Fetcher.StockProfile().Get("goog", "nasdaq");
		Assert.IsFalse(Result == null);
		Assert.IsTrue(Result.name == "Alphabet Inc.");
	}
	[TestMethod]
	public async Task DatabaseTest()
	{
		StockProfile Result = await new Data.Database.StockProfile().Get("goog", "nasdaq");
		Assert.IsFalse(Result == null);
		Assert.IsTrue(Result.name == "Alphabet Inc.");
	}
	[TestMethod]
	public async Task YahooTest()
	{
		StockProfile Result = await new Data.YahooFinance.StockProfile().Get("goog", "nasdaq");
		Assert.IsFalse(Result == null);
		Assert.IsTrue(Result.name == "Alphabet Inc.");
	}
}