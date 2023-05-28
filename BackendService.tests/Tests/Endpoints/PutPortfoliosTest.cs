namespace BackendService.tests;
using System.Data.SqlClient;

[TestClass]
public class PutPortfoliosTest
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
	public void PutPortfoliosTest_UpdateCurrencySuccessfullyTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.CreatePortfolioHelper(userTestObject);
		PutPortfoliosBody body = new PutPortfoliosBody("USD", "", portfolio.id!);
		PutPortfoliosResponse response = PutPortfolios.Endpoint(userTestObject.accessToken!, body);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.Portfolio updatedPortfolio = PortfolioHelper.GetPortfolio(portfolio.id!);
		Assert.IsTrue(updatedPortfolio.currency == "USD", "Portfolio currency should be USD but was " + updatedPortfolio.currency);
	}

	[TestMethod]
	public void PutPortfoliosTest_UpdateNameSuccessfullyTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.CreatePortfolioHelper(userTestObject);
		PutPortfoliosBody body = new PutPortfoliosBody("", "New Name Test", portfolio.id!);
		PutPortfoliosResponse response = PutPortfolios.Endpoint(userTestObject.accessToken!, body);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.Portfolio updatedPortfolio = PortfolioHelper.GetPortfolio(portfolio.id!);
		Assert.IsTrue(updatedPortfolio.name == "New Name Test", "Portfolio name should be New Name but was " + updatedPortfolio.name);
	}

	[TestMethod]
	public void PutPortfoliosTest_UpdateNameCurrencySuccessfullyTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.CreatePortfolioHelper(userTestObject);
		PutPortfoliosBody body = new PutPortfoliosBody("USD", "New Name Test", portfolio.id!);
		PutPortfoliosResponse response = PutPortfolios.Endpoint(userTestObject.accessToken!, body);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.Portfolio updatedPortfolio = PortfolioHelper.GetPortfolio(portfolio.id!);
		Assert.IsTrue(updatedPortfolio.name == "New Name Test", "Portfolio name should be New Name but was " + updatedPortfolio.name);
		Assert.IsTrue(updatedPortfolio.currency == "USD", "Portfolio currency should be USD but was " + updatedPortfolio.currency);
	}

	[TestMethod]
	public void PutPortfoliosTest_InvalidPortfolioIDTest()
	{
		PutPortfoliosBody body = new PutPortfoliosBody("invalid", "USD", "New Name Test");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutPortfolios.Endpoint(userTestObject.accessToken!, body));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutPortfoliosTest_InvalidUserTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.CreatePortfolioHelper(userTestObject);
		PutPortfoliosBody body = new PutPortfoliosBody(portfolio.id!, "USD", "New Name Test");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutPortfolios.Endpoint("invalid", body));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
		PortfolioHelper.DeletePortfolioHelper(portfolio, userTestObject);
	}
}