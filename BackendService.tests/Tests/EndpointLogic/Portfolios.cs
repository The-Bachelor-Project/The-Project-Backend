namespace BackendService.tests;
using API.v1;
[TestClass]
public class PortfoliosTest
{

	[TestMethod]
	public void PostPortfolioTest()
	{
		String name = "iewurhuieryh122112";
		String currency = "SEK";
		Decimal balance = 123.123m;
		PostPortfoliosBody body = new PostPortfoliosBody(new PortfolioBody(name, currency, balance, true), Assembly.accessToken);
		PostPortfoliosResponse response = PostPortfolios.Endpoint(body);
		Assembly.portfolioID = response.id;
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.id != "", "portfolio id should not be empty");
	}

	[TestMethod]
	public void GetSinglePortfolioTest()
	{
		GetPortfoliosResponse response = GetPortfolios.Endpoint(Assembly.portfolioID, Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 1, "number of portfolios should be 1 but was " + response.portfolios.Count);
		Assert.IsTrue(response.portfolios[0].id == Assembly.portfolioID, "the id of the portfolio should be " + Assembly.portfolioID + " but was " + response.portfolios[0].id);
		Assert.IsTrue(response.portfolios[0].name == "iewurhuieryh122112", "name of portfolio should be \"iewurhuieryh122112\" but was " + response.portfolios[0].name);
		Assert.IsTrue(response.portfolios[0].currency == "SEK", "portfolio currency should be \"SEK\" but was " + response.portfolios[0].currency);
		Assert.IsTrue(response.portfolios[0].balance == 123.123m, "balance of portfolio should be 123.123m but was " + response.portfolios[0].balance);
		Assert.IsTrue(response.portfolios[0].trackBalance == true, "trackBalance of portfolio should be true but was " + response.portfolios[0].trackBalance);

	}
}
