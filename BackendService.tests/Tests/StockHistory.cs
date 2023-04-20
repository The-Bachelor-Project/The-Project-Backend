using Data;

namespace BackendService.tests;

[TestClass]
public class StockHistoryTest
{
	[TestMethod]
	public async Task YahooTest()
	{
		StockHistory Result = await (new Data.YahooFinance.StockHistoryDaily()).Usd("vici", "nyse", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
		Assert.IsTrue(Result != null);
		Assert.IsTrue(Result.history.Length > 0);
		Assert.IsTrue(Result.history[0].Date < Result.history[5].Date);
	}
}