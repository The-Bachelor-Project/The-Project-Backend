namespace BackendService.tests;
using System.Data.SqlClient;

[TestClass]
public class GetPortfoliosTest
{
	private static UserTestObject userTestObject = new UserTestObject();

	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}



	[TestMethod]
	public void GetPortfoliosTest_SinglePortfolioTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		GetPortfoliosResponse response = GetPortfolios.Endpoint(portfolio.id!, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 1, "There should be 1 portfolio but there were " + response.portfolios.Count);
		Assert.IsTrue(response.portfolios[0].currency == "EUR", "Currency should be EUR but was " + response.portfolios[0].currency);
		PortfolioHelper.Delete(portfolio, userTestObject);
	}

	[TestMethod]
	public void GetPortfoliosTest_MultiplePortfoliosTest()
	{
		List<StockApp.Portfolio> portfolios = new List<StockApp.Portfolio>();
		for (int i = 0; i < 10; i++)
		{
			portfolios.Add(PortfolioHelper.Create(userTestObject));
		}
		GetPortfoliosResponse response = GetPortfolios.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 10, "There should be 10 portfolios but there were " + response.portfolios.Count);
		for (int i = 0; i < portfolios.Count; i++)
		{
			Assert.IsTrue(response.portfolios[i].currency == portfolios[i].currency, "Currency should be EUR but was " + response.portfolios[i].currency);
			Assert.IsTrue(response.portfolios[i].name == portfolios[i].name, "Portfolio name should be " + portfolios[i].name + " but was " + response.portfolios[i].name);
		}

		foreach (StockApp.Portfolio portfolio in portfolios)
		{
			PortfolioHelper.Delete(portfolio, userTestObject);
		}
	}

	[TestMethod]
	public void GetPortfoliosTest_NoPortfoliosTest()
	{
		GetPortfoliosResponse response = GetPortfolios.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 0, "There should be 0 portfolios but there were " + response.portfolios.Count);
	}

	[TestMethod]
	public void GetPortfoliosTest_InvalidTokenTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetPortfolios.Endpoint("invalid token"));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void GetPortfoliosTest_InvalidPortfolioIDOrOwner()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetPortfolios.Endpoint("", userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

}