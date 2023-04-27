namespace BackendService.tests;
using API.v1;
[TestClass]
public class PortfoliosTest
{
	private String[] names = new String[] { "test portfolio 1", "test portfolio 2", "test portfolio 3", "test portfolio 4" };
	private String[] currencies = new String[] { "SEK", "USD", "EUR", "GBX" };
	private Decimal[] balances = new Decimal[] { 123.123m, 456.456m, 789.789m, 101.101m };
	private Boolean[] trackBalances = new Boolean[] { true, false, true, false };

	[TestMethod]
	public void EndpointPostPortfolioTest()
	{
		for (int i = 0; i < names.Length; i++)
		{
			String name = names[i];
			String currency = currencies[i];
			Decimal balance = balances[i];
			Boolean trackBalance = trackBalances[i];
			PostPortfoliosBody body = new PostPortfoliosBody(new PortfolioBody(name, currency, balance, trackBalance), Assembly.accessToken);
			PostPortfoliosResponse response = PostPortfolios.Endpoint(body);
			Assembly.portfolioIds[i] = response.id;
			Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
			Assert.IsTrue(response.id != "", "portfolio id should not be empty");
		}

	}

	[TestMethod]
	public void EndpointGetSinglePortfolioTest()
	{
		GetPortfoliosResponse response = GetPortfolios.Endpoint(Assembly.portfolioIds[0], Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 1, "number of portfolios should be 1 but was " + response.portfolios.Count);
		Assert.IsTrue(response.portfolios[0].id == Assembly.portfolioIds[0], "the id of the portfolio should be " + Assembly.portfolioIds + " but was " + response.portfolios[0].id);
		Assert.IsTrue(response.portfolios[0].name == names[0], "name of portfolio should be " + names[0] + " but was " + response.portfolios[0].name);
		Assert.IsTrue(response.portfolios[0].currency == currencies[0], "portfolio currency should be " + currencies[0] + " but was " + response.portfolios[0].currency);
		Assert.IsTrue(response.portfolios[0].balance == balances[0], "balance of portfolio should be " + balances[0] + " but was " + response.portfolios[0].balance);
		Assert.IsTrue(response.portfolios[0].trackBalance == trackBalances[0], "trackBalance of portfolio should be " + trackBalances[0] + " but was " + response.portfolios[0].trackBalance);
	}

	[TestMethod]
	public void EndpointGetMultiplePortfolios()
	{
		GetPortfoliosResponse response = GetPortfolios.Endpoint(Assembly.accessToken);
		List<StockApp.Portfolio> sortedPortfolios = response.portfolios;
		sortedPortfolios.Sort((port1, port2) => port1.name.CompareTo(port2.name));
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 4, "number of portfolios should be 4 but was " + response.portfolios.Count);
		Assert.IsTrue(response.portfolios[2].name == names[2], "name of portfolio should be " + names[2] + " but was " + response.portfolios[2].name);
		Assert.IsTrue(response.portfolios[3].currency == currencies[3], "portfolio currency should be " + currencies[3] + " but was " + response.portfolios[3].currency);
		Assert.IsTrue(response.portfolios[1].balance == balances[1], "balance of portfolio should be " + balances[1] + " but was " + response.portfolios[1].balance);
	}
}
