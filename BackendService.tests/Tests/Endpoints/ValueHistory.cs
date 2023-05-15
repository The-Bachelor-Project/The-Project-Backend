namespace BackendService.tests;

[TestClass]
public class ValueHistoryTest
{
	//FIXME: This test fails due to max amount of connections
	[TestMethod, Priority(1)]
	public async Task EndpointGetValueHistory()
	{
		Data.UserAssetsValueHistory response = await new StockApp.EndpointHandler.ValueHistory(Assembly.accessToken).Get("USD", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2023-05-02"));
		Assert.IsTrue(response.portfolios.Count > 0, "response should have at least one portfolio");
		if (response.portfolios[0].positions.Count > 0)
		{
			Assert.IsTrue(response.portfolios[0].positions[0].valueHistory.Count > 0, "response should have at least one entry in value history");
			Assert.IsTrue(response.portfolios[0].positions[0].valueHistory[0].date == DateOnly.Parse("2021-01-01"), "response should have date 2021-01-01 but was " + response.portfolios[0].positions[0].valueHistory[0].date);
		}
		else if (response.portfolios[1].positions.Count > 0)
		{
			Assert.IsTrue(response.portfolios[1].positions[0].valueHistory.Count > 0, "response should have at least one entry in value history");
			Assert.IsTrue(response.portfolios[1].positions[0].valueHistory[0].date == DateOnly.Parse("2021-01-01"), "response should have date 2021-01-01 but was " + response.portfolios[1].positions[0].valueHistory[0].date);
		}
		else if (response.portfolios[2].positions.Count > 0)
		{
			Assert.IsTrue(response.portfolios[2].positions[0].valueHistory.Count > 0, "response should have at least one entry in value history");
			Assert.IsTrue(response.portfolios[2].positions[0].valueHistory[0].date == DateOnly.Parse("2021-01-01"), "response should have date 2021-01-01 but was " + response.portfolios[2].positions[0].valueHistory[0].date);
		}
		else if (response.portfolios[3].positions.Count > 0)
		{
			Assert.IsTrue(response.portfolios[3].positions[0].valueHistory.Count > 0, "response should have at least one entry in value history");
			Assert.IsTrue(response.portfolios[3].positions[0].valueHistory[0].date == DateOnly.Parse("2021-01-01"), "response should have date 2021-01-01 but was " + response.portfolios[3].positions[0].valueHistory[0].date);
		}
		else
		{
			Assert.Fail("response should have at least one position");
		}
		Assert.IsTrue(response.dividendHistory.Count > 0, "response should have at least one entry in dividend history");
	}
}