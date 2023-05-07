namespace BackendService.tests;
using Data;

[TestClass]
public class CurrencyFetcherTest
{
	[TestMethod, Priority(1)]
	public async Task SaveCurrencyHistoryToDBFromYFTest()
	{
		String code = "CAD";
		Boolean isReset = FetcherHelper.ResetHistory(code, "", 0);
		if (isReset)
		{
			CurrencyHistory result = await new Data.Fetcher.CurrencyFetcher().GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
			Assert.IsTrue(result != null, "Currency history is null");
			Assert.IsTrue(result.history.Count > 0, "Currency history is empty");
			Assert.IsTrue(result.history[0].date < result.history[5].date, "Currency history is not sorted correctly");
		}
		else
		{
			Assert.Fail("Currency history was not resetted correctly");
		}
	}

	[TestMethod, Priority(1)]
	public async Task CurrencyHistoryAlreadyInDBTest()
	{
		String code = "CAD";
		Boolean isSaved = await FetcherHelper.SaveCurrencyHistoryToDB(code);
		if (isSaved)
		{
			CurrencyHistory result = await new Data.Fetcher.CurrencyFetcher().GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
			Assert.IsTrue(result != null, "Currency history is null");
			Assert.IsTrue(result.history.Count > 0, "Currency history is empty");
			Assert.IsTrue(result.history[0].date < result.history[5].date, "Currency history is not sorted correctly");
		}
		else
		{
			Assert.Fail("Currency history was not added to DB correctly");
		}
	}
}