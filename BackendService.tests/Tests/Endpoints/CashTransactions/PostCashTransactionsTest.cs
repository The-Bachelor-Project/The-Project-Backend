namespace BackendService.tests;

[TestClass]
public class PostCashTransactionsTest
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
	public async Task PostCashTransactionsTest_SingleTransactionTest()
	{
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "CAD", 100, Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")), "BUY", "TEST");
		PostCashTransactionsResponse response = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Reponse should be success but was " + response.response);
		Assert.IsTrue(response.id != -1, "Id should not be -1");
	}

	[TestMethod]
	public async Task PostCashTransactionsTest_AllCurrenciesTest()
	{
		int tempBalance = 0;
		foreach (String currency in Dictionaries.currencies)
		{
			PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, currency, 100, Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")), "BUY", "TEST");
			PostCashTransactionsResponse response = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
			Assert.IsTrue(response.response == "success", "Reponse should be success but was " + response.response);
			Assert.IsTrue(response.id != -1, "Id should not be -1");
			StockApp.CashTransaction cashTransaction = CashTransactionHelper.Get(response.id);
			Assert.IsTrue(cashTransaction.balance!.amount > tempBalance, "Balance should be greater than " + tempBalance + " but was " + cashTransaction.balance.amount);
			Assert.IsTrue(cashTransaction.nativeAmount!.amount! == 100, "Native amount should be 100 but was " + cashTransaction.nativeAmount);
			Assert.IsTrue(cashTransaction.nativeAmount.currency == currency, "Native currency should be " + currency + " but was " + cashTransaction.nativeAmount.currency);
		}
	}

	[TestMethod]
	public async Task PostCashTransactionsTest_DifferentOrderTest()
	{
		int timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "CAD", 100, timestamp, "BUY", "TEST");
		PostCashTransactionsResponse responseOne = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseOne.response == "success", "Reponse should be success but was " + responseOne.response);
		Assert.IsTrue(responseOne.id != -1, "Id should not be -1");
		body = new PostCashTransactionsBody(portfolio!.id!, "DKK", 100, timestamp + 10, "BUY", "TEST");
		PostCashTransactionsResponse responseTwo = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseTwo.response == "success", "Reponse should be success but was " + responseTwo.response);
		Assert.IsTrue(responseTwo.id != -1, "Id should not be -1");
		body = new PostCashTransactionsBody(portfolio!.id!, "EUR", 100, timestamp + 5, "BUY", "TEST");
		PostCashTransactionsResponse responseThree = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseThree.response == "success", "Reponse should be success but was " + responseThree.response);
		Assert.IsTrue(responseThree.id != -1, "Id should not be -1");
		body = new PostCashTransactionsBody(portfolio!.id!, "JPY", 100, timestamp - 20, "BUY", "TEST");
		PostCashTransactionsResponse responseFour = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseFour.response == "success", "Reponse should be success but was " + responseFour.response);
		Assert.IsTrue(responseFour.id != -1, "Id should not be -1");

		StockApp.CashTransaction cashTransactionOne = CashTransactionHelper.Get(responseOne.id);
		StockApp.CashTransaction cashTransactionTwo = CashTransactionHelper.Get(responseTwo.id);
		StockApp.CashTransaction cashTransactionThree = CashTransactionHelper.Get(responseThree.id);
		StockApp.CashTransaction cashTransactionFour = CashTransactionHelper.Get(responseFour.id);
		Assert.IsTrue(cashTransactionFour.balance!.amount < cashTransactionOne.balance!.amount &&
						cashTransactionOne.balance!.amount < cashTransactionThree.balance!.amount &&
						cashTransactionThree.balance!.amount < cashTransactionTwo.balance!.amount,
						"Balance should be in order but had the values, One: " + cashTransactionOne.balance!.amount +
						", Two: " + cashTransactionTwo.balance!.amount +
						", Three: " + cashTransactionThree.balance!.amount +
						", Four: " + cashTransactionFour.balance!.amount);
	}

	[TestMethod]
	public async Task PostCashTransactionsTest_BothPositiveAndNegativeTransactionsTest()
	{
		int timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "CAD", -30, timestamp, "BUY", "TEST");
		PostCashTransactionsResponse responseOne = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseOne.response == "success", "Reponse should be success but was " + responseOne.response);
		Assert.IsTrue(responseOne.id != -1, "Id should not be -1");
		body = new PostCashTransactionsBody(portfolio!.id!, "DKK", 2000, timestamp + 10, "BUY", "TEST");
		PostCashTransactionsResponse responseTwo = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseTwo.response == "success", "Reponse should be success but was " + responseTwo.response);
		Assert.IsTrue(responseTwo.id != -1, "Id should not be -1");
		body = new PostCashTransactionsBody(portfolio!.id!, "EUR", -50, timestamp + 5, "BUY", "TEST");
		PostCashTransactionsResponse responseThree = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseThree.response == "success", "Reponse should be success but was " + responseThree.response);
		Assert.IsTrue(responseThree.id != -1, "Id should not be -1");
		body = new PostCashTransactionsBody(portfolio!.id!, "PLN", 200, timestamp - 20, "BUY", "TEST");
		PostCashTransactionsResponse responseFour = await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(responseFour.response == "success", "Reponse should be success but was " + responseFour.response);
		Assert.IsTrue(responseFour.id != -1, "Id should not be -1");

		StockApp.CashTransaction cashTransactionOne = CashTransactionHelper.Get(responseOne.id);
		StockApp.CashTransaction cashTransactionTwo = CashTransactionHelper.Get(responseTwo.id);
		StockApp.CashTransaction cashTransactionThree = CashTransactionHelper.Get(responseThree.id);
		StockApp.CashTransaction cashTransactionFour = CashTransactionHelper.Get(responseFour.id);
		Assert.IsTrue(cashTransactionFour.balance!.amount > cashTransactionOne.balance!.amount &&
		cashTransactionOne.balance!.amount > cashTransactionThree.balance!.amount &&
		cashTransactionThree.balance!.amount < cashTransactionTwo.balance!.amount,
						"Balance should be in order but had the values, One: " + cashTransactionOne.balance!.amount +
						", Two: " + cashTransactionTwo.balance!.amount +
						", Three: " + cashTransactionThree.balance!.amount +
						", Four: " + cashTransactionFour.balance!.amount);
	}

	[TestMethod]
	public async Task PostCashTransactionsTest_MissingValuesTest()
	{
		// Currency
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "", 100, 10000, "BUY", "TEST");
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Native amount
		body = new PostCashTransactionsBody(portfolio!.id!, "DKK", 0, 10000, "BUY", "TEST");
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Timestamp
		body = new PostCashTransactionsBody(portfolio!.id!, "DKK", 100, 0, "BUY", "TEST");
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Type
		body = new PostCashTransactionsBody(portfolio!.id!, "DKK", 100, 10000, "", "TEST");
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Portfolio
		body = new PostCashTransactionsBody("", "DKK", 100, 10000, "BUY", "TEST");
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostCashTransactionsTest_NullValuesTest()
	{
		// Currency
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, null!, 100, 10000, "BUY", "TEST");
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Type
		body = new PostCashTransactionsBody(portfolio!.id!, "DKK", 100, 10000, null!, "TEST");
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Portfolio
		body = new PostCashTransactionsBody(null!, "DKK", 100, 10000, "BUY", "TEST");
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostCashTransactionsTest_InvalidCurrencyTest()
	{
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "invalid", 100, 10000, "BUY", "TEST");
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostCashTransactionsTest_InvalidOwnerTest()
	{
		UserTestObject userTestObjectTwo = UserHelper.Create();
		StockApp.Portfolio portfolioTwo = PortfolioHelper.Create(userTestObjectTwo);
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolioTwo.id!, "DKK", 100, 10000, "BUY", "TEST");
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostCashTransactions.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 403, "Status code should be 403 but was " + exception.StatusCode);
		UserHelper.Delete(userTestObjectTwo);
	}
}