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
			Assert.IsTrue(result != null, "Currency history is null");
			Assert.IsTrue(result.history.Count > 0, "Currency history is empty");
			Assert.IsTrue(result.history[0].date < result.history[5].date, "Currency history is not sorted correctly");
		}
		else
		{
			Assert.Fail("Currency history was not saved to database from either YF or DB");
		}
	}

	[TestMethod]
	public async Task DatabaseGBXHistoryFetcherTest()
	{
		String code = "GBX";
		Boolean isSaved = await FetcherHelper.SaveCurrencyHistoryToDB(code);
		if (isSaved)
		{
			CurrencyHistory result = await new Data.Fetcher.DatabaseFetcher.CurrencyFetcher().GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
			Assert.IsTrue(result.currency == code, "Currency code is not correct, should be " + code + " but is " + result.currency);
			Assert.IsTrue(result != null, "Currency history is null");
			Assert.IsTrue(result.history.Count > 0, "Currency history is empty");
			Assert.IsTrue(result.history[0].date < result.history[5].date, "Currency history is not sorted correctly");
		}
		else
		{
			Assert.Fail("Currency history was not saved to database from either YF or DB");
		}
	}
}