using BackendService.tests;
using Data;

[TestClass]
public class YFCurrencyFetcherTest
{
	public Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher fetcher = new();

	// Codes is missing GBX, as this can not be gotten from YF.
	[TestMethod]
	public async Task YFCurrenciesFetcher()
	{
		String[] codes = new string[] { "USD", "DKK", "SEK", "NOK", "EUR", "CAD", "GBP" };
		List<String> problemCurrencies = new List<String>();

		foreach (String code in codes)
		{
			try
			{
				CurrencyHistory result = await fetcher.GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
				Assert.IsTrue(result != null);
				Assert.IsTrue(result.history.Count > 0);
				Assert.IsTrue(result.history[0].date < result.history[5].date);
			}
			catch (AssertFailedException)
			{
				problemCurrencies.Add(code);
			}
		}

		if (problemCurrencies.Count > 0)
		{
			Assert.Fail("The following currencies failed: " + String.Join(",", problemCurrencies));
		}
	}
}