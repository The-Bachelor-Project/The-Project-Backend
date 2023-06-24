namespace BackendService.tests;

[TestClass]
public class DeleteStockTransactionsTest
{
	private static UserTestObject userTestObject = new UserTestObject();
	private static StockApp.StockTransaction stockTransaction = null!;
	[TestInitialize]
	public async Task Initialize()
	{
		userTestObject = UserHelper.Create();
		stockTransaction = new StockApp.StockTransaction();
		stockTransaction = await StockTransactionHelper.Create(userTestObject);

	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public async Task DeleteStockTransactionsTest_DeleteTest()
	{
		DeleteStockTransactionsResponse response = await DeleteStockTransactions.Endpoint(userTestObject.accessToken!, StockTransactionHelper.Get((int)stockTransaction.id!).portfolioId!, (int)stockTransaction.id!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
	}

	[TestMethod]
	public async Task DeleteStockTransactionsTest_InvalidUserTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => DeleteStockTransactions.Endpoint("invalid", StockTransactionHelper.Get((int)stockTransaction.id!).portfolioId!, (int)stockTransaction.id!));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DeleteStockTransactionsTest_InvalidPortfolioTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => DeleteStockTransactions.Endpoint(userTestObject.accessToken!, "invalid", (int)stockTransaction.id!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DeleteStockTransactionsTest_InvalidTransactionTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => DeleteStockTransactions.Endpoint(userTestObject.accessToken!, StockTransactionHelper.Get((int)stockTransaction.id!).portfolioId!, 0));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DeleteStockTransactionsTest_InvalidPortfolioAndUserTest()
	{
		UserTestObject tempUser = UserHelper.Create();
		StockApp.Portfolio portfolio = PortfolioHelper.Create(tempUser);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => DeleteStockTransactions.Endpoint(userTestObject.accessToken!, portfolio.id!, (int)stockTransaction.id!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
		UserHelper.Delete(tempUser);
	}
}