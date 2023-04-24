namespace BackendService.tests;
using Data;

[TestClass]
public class CurrencyFetcherTest
{
	[TestMethod]
	public async Task SaveCurrencyHistoryToDBFromYFTest()
	{
		String code = "CAD";
		Boolean isReset = FetcherHelper.ResetHistory(code, "", 0);
		if (isReset)
		{
			CurrencyHistory result = await new Data.Fetcher.CurrencyFetcher().GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.history.Count > 0);
			Assert.IsTrue(result.history[0].date < result.history[5].date);
		}
		else
		{
			Assert.Fail("Currency history was not resetted correctly");
		}
	}

	[TestMethod]
	public async Task CurrencyHistoryAlreadyInDBTest()
	{
		String code = "CAD";
		Boolean isSaved = await FetcherHelper.SaveCurrencyHistoryToDB(code);
		if (isSaved)
		{
			CurrencyHistory result = await new Data.Fetcher.CurrencyFetcher().GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.history.Count > 0);
			Assert.IsTrue(result.history[0].date < result.history[5].date);
		}
		else
		{
			Assert.Fail("Currency history was not added to DB correctly");
		}
	}
}