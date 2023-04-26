namespace BackendService.tests;

[TestClass]
public class PortfoliosTest
{

	[TestMethod]
	public void PostPortfolioTest()
	{
		String name = "iewurhuieryh122112";
		String currency = "SEK";
		Decimal balance = 123.123m;
		StockApp.Portfolio portfolio = new StockApp.Portfolio(name, Assembly.user!.id!, currency, balance, true);
		portfolio.AddToDb();
		Assembly.portfolioID = portfolio.id!;
		Assert.IsTrue(portfolio.id != null, "Portfolio id should not be null");
		Assert.IsTrue(portfolio.stockTransactions.Count() == 0, "Portfolio stockTransactions should be empty but is " + portfolio.stockTransactions.Count());
		Assert.IsTrue(portfolio.stockPositions.Count() == 0, "Portfolio stockPositions should be empty but is " + portfolio.stockPositions.Count());
	}

	[TestMethod]
	public void GetSinglePortfolioTest()
	{
		StockApp.Portfolio portfolio = Assembly.user!.GetPortfolio(Assembly.portfolioID);
		Assert.IsTrue(portfolio.id == Assembly.portfolioID, "Portfolio id should be " + Assembly.portfolioID + " but was " + portfolio.id);
		Assert.IsTrue(portfolio.owner == Assembly.user!.id, "Portfolio owner should be " + Assembly.user!.id + " but was " + portfolio.owner);
		Assert.IsTrue(portfolio.name == "iewurhuieryh122112", "Portfolio name should be \"iewurhuieryh122112\" but was " + portfolio.name);
	}
}
