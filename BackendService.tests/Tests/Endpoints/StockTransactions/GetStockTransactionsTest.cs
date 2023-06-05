namespace BackendService.tests;

[TestClass]
public class GetStockTransactionsTest
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
	public void GetStockTransactionsTest_GetSingleTest()
	{
		GetStockTransactionsResponse response = GetStockTransactions.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stockTransactions.Count == 1, "Response should contain 1 stockTransaction but contained " + response.stockTransactions.Count);
	}

	[TestMethod]
	public async Task GetStockTransactionsTest_GetMultipleTest()
	{
		StockApp.StockTransaction stockTransaction2 = new StockApp.StockTransaction();
		stockTransaction2 = await StockTransactionHelper.Create(userTestObject);
		GetStockTransactionsResponse response = GetStockTransactions.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stockTransactions.Count == 2, "Response should contain 2 stockTransactions but contained " + response.stockTransactions.Count);
	}

	[TestMethod]
	public void GetStockTransactionsTest_GetEmpty()
	{
		UserTestObject tempUser = UserHelper.Create();
		GetStockTransactionsResponse response = GetStockTransactions.Endpoint(tempUser.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stockTransactions.Count == 0, "Response should contain 0 stockTransactions but contained " + response.stockTransactions.Count);
		UserHelper.Delete(tempUser);
	}

	[TestMethod]
	public void GetStockTransactionsTest_InvalidUser()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetStockTransactions.Endpoint("invalid"));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}
}