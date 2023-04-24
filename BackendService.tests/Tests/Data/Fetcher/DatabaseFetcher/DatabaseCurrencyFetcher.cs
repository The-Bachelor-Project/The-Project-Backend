namespace BackendService.tests;
using Data;

[TestClass]
public class DatabaseCurrencyFethcer
{
	[TestMethod]
	public async Task DatabaseCurrencyHistoryFetcherTest()
	{
		String code = "EUR";
		Boolean isSaved = await FetcherHelper.SaveCurrencyHistoryToDB(code);
		if (isSaved)
		{
			CurrencyHistory result = await new Data.Fetcher.DatabaseFetcher.CurrencyFetcher().GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.history.Count > 0);
			Assert.IsTrue(result.history[0].date < result.history[5].date);
		}
		else
		{
			Assert.Fail("Currency history was not saved to database from either YF or DB");
		}
	}
}