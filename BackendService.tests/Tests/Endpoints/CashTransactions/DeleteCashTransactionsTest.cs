namespace BackendService.tests;

[TestClass]
public class DeleteCashTransactionsTest
{
	private UserTestObject? userTestObject;
	private StockApp.Portfolio? portfolio;

	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
		portfolio = PortfolioHelper.Create(userTestObject);
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject!);
	}

	[TestMethod]
	public async Task DeleteCashTransactionsTest_SuccessfulDeletionTest()
	{
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "CAD", 100, Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")), "Deposit", "TEST");
		PostCashTransactionsResponse postResponse = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		DeleteCashTransactionsResponse deleteResponse = DeleteCashTransactions.Endpoint(portfolio!.id!, postResponse.id!, userTestObject!.accessToken!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => CashTransactionHelper.Get(postResponse.id!));
		Assert.IsTrue(deleteResponse.response == "success", "Response should be success but was " + deleteResponse.response);
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);

	}

	[TestMethod]
	public async Task DeleteCashTransactionsTest_InvalidPortfolioTest()
	{
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "CAD", 100, Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")), "Deposit", "TEST");
		PostCashTransactionsResponse postResponse = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint("invalid", postResponse.id!, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DeleteCashTransactionsTest_WrongPortoflioTest()
	{
		StockApp.Portfolio tempPortfolio = PortfolioHelper.Create(userTestObject!);
		PostCashTransactionsBody body = new PostCashTransactionsBody(tempPortfolio.id!, "CAD", 100, Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")), "Deposit", "TEST");
		PostCashTransactionsResponse postResponse = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint(portfolio!.id!, postResponse.id, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task DeleteCashTransactionsTest_WrongUserTest()
	{
		UserTestObject tempUser = UserHelper.Create();
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "CAD", 100, Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")), "Deposit", "TEST");
		PostCashTransactionsResponse postResponse = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint(portfolio!.id!, postResponse.id, tempUser.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void DeleteCashTransactionsTest_NonExistigCashTransactionTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint(portfolio!.id!, -999, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void DeleteCashTransactionsTest_MissingValuesTest()
	{
		// Portfolio id
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint("", 1, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Access token
		exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint(portfolio!.id!, 1, ""));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void DeleteCashTransactionsTest_NullValuesTest()
	{
		// Portfolio id
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint(null!, 1, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Access token
		exception = Assert.ThrowsException<StatusCodeException>(() => DeleteCashTransactions.Endpoint(portfolio!.id!, 1, null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}
}