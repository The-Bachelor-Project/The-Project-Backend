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
		Assert.IsTrue(portfolio.id != null);
		Assert.IsTrue(portfolio.stockTransactions.Count() == 0);
		Assert.IsTrue(portfolio.stockPositions.Count() == 0);
	}

	[TestMethod]
	public void GetSinglePortfolioTest()
	{
		StockApp.Portfolio portfolio = Assembly.user!.GetPortfolio(Assembly.portfolioID);
		Assert.IsTrue(portfolio.id == Assembly.portfolioID);
		Assert.IsTrue(portfolio.owner == Assembly.user!.id);
		Assert.IsTrue(portfolio.name == "iewurhuieryh122112");
	}
}
