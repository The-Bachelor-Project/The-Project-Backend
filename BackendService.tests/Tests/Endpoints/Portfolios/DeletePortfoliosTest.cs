namespace BackendService.tests;

[TestClass]
public class DeletePortfoliosTest
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
	public void DeletePortfoliosTest_SuccessfulDeletionTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		DeletePortfoliosResponse response = DeletePortfolios.Endpoint(portfolio.id!, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => GetPortfolios.Endpoint(portfolio.id!, userTestObject.accessToken!));
		Assert.IsTrue(statusCodeException.StatusCode == 404, "Status code should be 404 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeletePortfoliosTest_InvalidPortfolioIDTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeletePortfolios.Endpoint("invalid", userTestObject.accessToken!));
		Assert.IsTrue(statusCodeException.StatusCode == 404, "Status code should be 404 but was " + statusCodeException.StatusCode);

	}

	[TestMethod]
	public void DeletePortfoliosTest_InvalidUserIDTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeletePortfolios.Endpoint("invalid", userTestObject.accessToken!));
		Assert.IsTrue(statusCodeException.StatusCode == 404, "Status code should be 404 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeletePortfoliosTest_NullAccessTokenTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeletePortfolios.Endpoint("invalid", null!));
		Assert.IsTrue(statusCodeException.StatusCode == 400, "Status code should be 400 but was " + statusCodeException.StatusCode);
	}

}