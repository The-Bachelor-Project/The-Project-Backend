namespace BackendService.tests;

[TestClass]
public class PutCashTransactionsTest
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
	public async Task PutCashTransactionsTest_SuccessfulIndividualValuesTest()
	{
		StockApp.CashTransaction cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction.type = "Deposit";
		cashTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction.balance = new StockApp.Money(350, "USD");
		cashTransaction.description = "Test";
		await cashTransaction.AddToDb();

		// Change description
		PutCashTransactionsBody body = new PutCashTransactionsBody((int)cashTransaction.id!, portfolio.id!, "", 0, "New description", 0);
		PutCashTransactionsResponse response = await PutCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.CashTransaction gottenCashTransaction = CashTransactionHelper.Get(response.id);
		Assert.IsTrue(gottenCashTransaction.description == "New description", "Description should be New description but was " + gottenCashTransaction.description);
		cashTransaction.id = gottenCashTransaction.id;

		// Change native currency
		body = new PutCashTransactionsBody((int)cashTransaction.id!, portfolio.id!, "DKK", 0, "", 0);
		response = await PutCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		gottenCashTransaction = CashTransactionHelper.Get(response.id);
		Assert.IsTrue(gottenCashTransaction.nativeAmount!.currency == "DKK", "Native currency should be DKK but was " + gottenCashTransaction.nativeAmount!.currency);
		cashTransaction.id = gottenCashTransaction.id;

		// Change native amount
		body = new PutCashTransactionsBody((int)cashTransaction.id!, portfolio.id!, "", 500, "", 0);
		response = await PutCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		gottenCashTransaction = CashTransactionHelper.Get(response.id);
		Assert.IsTrue(gottenCashTransaction.nativeAmount!.amount == 500, "Native amount should be 500 but was " + gottenCashTransaction.nativeAmount!.amount);
		cashTransaction.id = gottenCashTransaction.id;

		// Change timestamp
		int newTimestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now) - 500;
		body = new PutCashTransactionsBody((int)cashTransaction.id!, portfolio.id!, "", 0, "", newTimestamp);
		response = await PutCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		gottenCashTransaction = CashTransactionHelper.Get(response.id);
		Assert.IsTrue(gottenCashTransaction.timestamp == newTimestamp, "Timestamp should be " + newTimestamp + " but was " + gottenCashTransaction.timestamp);
		cashTransaction.id = gottenCashTransaction.id;
	}

	[TestMethod]
	public async Task PutCashTransactionsTest_ChangeEverythingTogetherTest()
	{
		StockApp.CashTransaction cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction.type = "Deposit";
		cashTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction.balance = new StockApp.Money(350, "USD");
		cashTransaction.description = "Test";
		await cashTransaction.AddToDb();

		PutCashTransactionsBody body = new PutCashTransactionsBody((int)cashTransaction.id!, portfolio.id!, "DKK", 500, "New description", (int)cashTransaction.timestamp - 500);
		PutCashTransactionsResponse response = await PutCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.CashTransaction gottenCashTransaction = CashTransactionHelper.Get(response.id);
		Assert.IsTrue(gottenCashTransaction.nativeAmount!.currency == "DKK", "Native currency should be DKK but was " + gottenCashTransaction.nativeAmount!.currency);
		Assert.IsTrue(gottenCashTransaction.nativeAmount!.amount == 500, "Native amount should be 500 but was " + gottenCashTransaction.nativeAmount!.amount);
		Assert.IsTrue(gottenCashTransaction.timestamp == ((int)cashTransaction.timestamp - 500), "Timestamp should be 500 but was " + gottenCashTransaction.timestamp);
		Assert.IsTrue(gottenCashTransaction.description == "New description", "Description should be New description but was " + gottenCashTransaction.description);
	}

	[TestMethod]
	public void PutCashTransactionsTest_MissingValuesTest()
	{
		// Portfolio id
		PutCashTransactionsBody body = new PutCashTransactionsBody(0, "", "DKK", 500, "New description", 500);
		StatusCodeException exception = Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PutCashTransactions.Endpoint(body, userTestObject!.accessToken!)).Result;
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Access Token
		body = new PutCashTransactionsBody(0, portfolio!.id!, "DKK", 500, "New description", 500);
		exception = Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PutCashTransactions.Endpoint(body, "")).Result;
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutCashTransactionsTest_NullValuesTest()
	{
		// Portoflio id
		PutCashTransactionsBody body = new PutCashTransactionsBody(0, null!, "DKK", 500, "New description", 500);
		StatusCodeException exception = Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PutCashTransactions.Endpoint(body, userTestObject!.accessToken!)).Result;
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Access token
		body = new PutCashTransactionsBody(0, portfolio!.id!, "DKK", 500, "New description", 500);
		exception = Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PutCashTransactions.Endpoint(body, null!)).Result;
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

	}



}