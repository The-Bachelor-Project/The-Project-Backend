namespace BackendService.tests;

[TestClass]
public class GetCashTransactionsTest
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
	public async Task GetCashTransactionsTest_GetSingleTest()
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
		GetCashTransactionsResponse response = GetCashTransactions.Endpoint(userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.cashTransactions.Count == 1, "There should be 1 cash transaction but there were " + response.cashTransactions.Count);
		Assert.IsTrue(response.cashTransactions[0].id == cashTransaction.id, "The cash transaction id should be " + cashTransaction.id + " but was " + response.cashTransactions[0].id);
		Assert.IsTrue(response.cashTransactions[0].portfolioId == cashTransaction.portfolioId, "The cash transaction portfolio id should be " + cashTransaction.portfolioId + " but was " + response.cashTransactions[0].portfolioId);
		Assert.IsTrue(response.cashTransactions[0].nativeAmount!.amount == cashTransaction.nativeAmount.amount, "The cash transaction native amount should be " + cashTransaction.nativeAmount.amount + " but was " + response.cashTransactions[0].nativeAmount!.amount);
		Assert.IsTrue(response.cashTransactions[0].nativeAmount!.currency == cashTransaction.nativeAmount.currency, "The cash transaction native currency should be " + cashTransaction.nativeAmount.currency + " but was " + response.cashTransactions[0].nativeAmount!.currency);
		Assert.IsTrue(response.cashTransactions[0].usdAmount!.currency == cashTransaction.usdAmount.currency, "The cash transaction usd currency should be " + cashTransaction.usdAmount.currency + " but was " + response.cashTransactions[0].usdAmount!.currency);
		Assert.IsTrue(response.cashTransactions[0].type == cashTransaction.type, "The cash transaction type should be " + cashTransaction.type + " but was " + response.cashTransactions[0].type);
		Assert.IsTrue(response.cashTransactions[0].timestamp == cashTransaction.timestamp, "The cash transaction timestamp should be " + cashTransaction.timestamp + " but was " + response.cashTransactions[0].timestamp);
	}

	[TestMethod]
	public async Task GetCashTransactionsTest_GetMultipleTest()
	{
		StockApp.CashTransaction cashTransaction1 = new StockApp.CashTransaction();
		cashTransaction1.portfolioId = portfolio!.id;
		cashTransaction1.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction1.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction1.type = "Deposit";
		cashTransaction1.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction1.balance = new StockApp.Money(350, "USD");
		cashTransaction1.description = "Test";
		await cashTransaction1.AddToDb();
		StockApp.CashTransaction cashTransaction2 = new StockApp.CashTransaction();
		cashTransaction2.portfolioId = portfolio!.id;
		cashTransaction2.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction2.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction2.type = "Deposit";
		cashTransaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction2.balance = new StockApp.Money(350, "USD");
		cashTransaction2.description = "Test";
		await cashTransaction2.AddToDb();
		GetCashTransactionsResponse response = GetCashTransactions.Endpoint(userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.cashTransactions.Count == 2, "There should be 2 cash transactions but there were " + response.cashTransactions.Count);
		Assert.IsTrue(response.cashTransactions[0].id == cashTransaction1.id || response.cashTransactions[0].id == cashTransaction2.id, "The cash transaction id should be " + cashTransaction1.id + " or " + cashTransaction2.id + " but was " + response.cashTransactions[0].id);
		Assert.IsTrue(response.cashTransactions[0].portfolioId == cashTransaction1.portfolioId || response.cashTransactions[0].portfolioId == cashTransaction2.portfolioId, "The cash transaction portfolio id should be " + cashTransaction1.portfolioId + " or " + cashTransaction2.portfolioId + " but was " + response.cashTransactions[0].portfolioId);
		Assert.IsTrue(response.cashTransactions[0].nativeAmount!.amount == cashTransaction1.nativeAmount.amount || response.cashTransactions[0].nativeAmount!.amount == cashTransaction2.nativeAmount.amount, "The cash transaction native amount should be " + cashTransaction1.nativeAmount.amount + " or " + cashTransaction2.nativeAmount.amount + " but was " + response.cashTransactions[0].nativeAmount!.amount);
		Assert.IsTrue(response.cashTransactions[0].nativeAmount!.currency == cashTransaction1.nativeAmount.currency || response.cashTransactions[0].nativeAmount!.currency == cashTransaction2.nativeAmount.currency, "The cash transaction native currency should be " + cashTransaction1.nativeAmount.currency + " or " + cashTransaction2.nativeAmount.currency + " but was " + response.cashTransactions[0].nativeAmount!.currency);
		Assert.IsTrue(response.cashTransactions[0].usdAmount!.currency == cashTransaction1.usdAmount.currency || response.cashTransactions[0].usdAmount!.currency == cashTransaction2.usdAmount.currency, "The cash transaction usd currency should be " + cashTransaction1.usdAmount.currency + " or " + cashTransaction2.usdAmount.currency + " but was " + response.cashTransactions[0].usdAmount!.currency);
		Assert.IsTrue(response.cashTransactions[0].type == cashTransaction1.type || response.cashTransactions[0].type == cashTransaction2.type, "The cash transaction type should be " + cashTransaction1.type + " or " + cashTransaction2.type + " but was " + response.cashTransactions[0].type);
		Assert.IsTrue(response.cashTransactions[0].timestamp == cashTransaction1.timestamp || response.cashTransactions[0].timestamp == cashTransaction2.timestamp, "The cash transaction timestamp should be " + cashTransaction1.timestamp + " or " + cashTransaction2.timestamp + " but was " + response.cashTransactions[0].timestamp);
		Assert.IsTrue(response.cashTransactions[0].description == cashTransaction1.description || response.cashTransactions[0].description == cashTransaction2.description, "The cash transaction description should be " + cashTransaction1.description + " or " + cashTransaction2.description + " but was " + response.cashTransactions[0].description);
		Assert.IsTrue(response.cashTransactions[1].id == cashTransaction1.id || response.cashTransactions[1].id == cashTransaction2.id, "The cash transaction id should be " + cashTransaction1.id + " or " + cashTransaction2.id + " but was " + response.cashTransactions[1].id);
		Assert.IsTrue(response.cashTransactions[1].portfolioId == cashTransaction1.portfolioId || response.cashTransactions[1].portfolioId == cashTransaction2.portfolioId, "The cash transaction portfolio id should be " + cashTransaction1.portfolioId + " or " + cashTransaction2.portfolioId + " but was " + response.cashTransactions[1].portfolioId);
		Assert.IsTrue(response.cashTransactions[1].nativeAmount!.amount == cashTransaction1.nativeAmount.amount || response.cashTransactions[1].nativeAmount!.amount == cashTransaction2.nativeAmount.amount, "The cash transaction native amount should be " + cashTransaction1.nativeAmount.amount + " or " + cashTransaction2.nativeAmount.amount + " but was " + response.cashTransactions[1].nativeAmount!.amount);
		Assert.IsTrue(response.cashTransactions[1].nativeAmount!.currency == cashTransaction1.nativeAmount.currency || response.cashTransactions[1].nativeAmount!.currency == cashTransaction2.nativeAmount.currency, "The cash transaction native currency should be " + cashTransaction1.nativeAmount.currency + " or " + cashTransaction2.nativeAmount.currency + " but was " + response.cashTransactions[1].nativeAmount!.currency);
		Assert.IsTrue(response.cashTransactions[1].usdAmount!.currency == cashTransaction1.usdAmount.currency || response.cashTransactions[1].usdAmount!.currency == cashTransaction2.usdAmount.currency, "The cash transaction usd currency should be " + cashTransaction1.usdAmount.currency + " or " + cashTransaction2.usdAmount.currency + " but was " + response.cashTransactions[1].usdAmount!.currency);
		Assert.IsTrue(response.cashTransactions[1].type == cashTransaction1.type || response.cashTransactions[1].type == cashTransaction2.type, "The cash transaction type should be " + cashTransaction1.type + " or " + cashTransaction2.type + " but was " + response.cashTransactions[1].type);
		Assert.IsTrue(response.cashTransactions[1].timestamp == cashTransaction1.timestamp || response.cashTransactions[1].timestamp == cashTransaction2.timestamp, "The cash transaction timestamp should be " + cashTransaction1.timestamp + " or " + cashTransaction2.timestamp + " but was " + response.cashTransactions[1].timestamp);
		Assert.IsTrue(response.cashTransactions[1].description == cashTransaction1.description || response.cashTransactions[1].description == cashTransaction2.description, "The cash transaction description should be " + cashTransaction1.description + " or " + cashTransaction2.description + " but was " + response.cashTransactions[1].description);
	}

	[TestMethod]
	public void GetCashTransactionsTest_GetEmptyTest()
	{
		GetCashTransactionsResponse response = GetCashTransactions.Endpoint(userTestObject!.accessToken!);
		Assert.IsTrue(response.cashTransactions.Count == 0, "The cash transactions count should be 0 but was " + response.cashTransactions.Count);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
	}

	[TestMethod]
	public async Task GetCashTransactionsTest_GetEveryTypeTest()
	{
		StockApp.CashTransaction cashTransaction1 = new StockApp.CashTransaction();
		cashTransaction1.portfolioId = portfolio!.id;
		cashTransaction1.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction1.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction1.type = "Deposit";
		cashTransaction1.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction1.balance = new StockApp.Money(350, "USD");
		cashTransaction1.description = "Test";
		await cashTransaction1.AddToDb();
		StockApp.CashTransaction cashTransaction2 = new StockApp.CashTransaction();
		cashTransaction2.portfolioId = portfolio!.id;
		cashTransaction2.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction2.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction2.type = "Withdrawal";
		cashTransaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction2.balance = new StockApp.Money(250, "USD");
		cashTransaction2.description = "Test";
		await cashTransaction2.AddToDb();
		StockApp.CashTransaction cashTransaction3 = new StockApp.CashTransaction();
		cashTransaction3.portfolioId = portfolio!.id;
		cashTransaction3.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction3.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction3.type = "Stock BUY";
		cashTransaction3.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction3.balance = new StockApp.Money(150, "USD");
		cashTransaction3.description = "Test";
		await cashTransaction3.AddToDb();
		StockApp.CashTransaction cashTransaction4 = new StockApp.CashTransaction();
		cashTransaction4.portfolioId = portfolio!.id;
		cashTransaction4.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction4.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction4.type = "Stock SELL";
		cashTransaction4.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction4.balance = new StockApp.Money(50, "USD");
		cashTransaction4.description = "Test";
		await cashTransaction4.AddToDb();
		StockApp.CashTransaction cashTransaction5 = new StockApp.CashTransaction();
		cashTransaction5.portfolioId = portfolio!.id;
		cashTransaction5.nativeAmount = new StockApp.Money(100, "CAD");
		cashTransaction5.usdAmount = new StockApp.Money(100, "USD");
		cashTransaction5.type = "Dividend";
		cashTransaction5.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction5.balance = new StockApp.Money(0, "USD");
		cashTransaction5.description = "Test";
		await cashTransaction5.AddToDb();

		GetCashTransactionsResponse response = GetCashTransactions.Endpoint(userTestObject!.accessToken!);
		Assert.IsTrue(response.cashTransactions.Count == 5, "The cash transactions count should be 5 but was " + response.cashTransactions.Count);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.cashTransactions[0].type == cashTransaction1.type, "The cash transaction type should be " + cashTransaction1.type + " but was " + response.cashTransactions[0].type);
		Assert.IsTrue(response.cashTransactions[1].type == cashTransaction2.type, "The cash transaction type should be " + cashTransaction2.type + " but was " + response.cashTransactions[1].type);
		Assert.IsTrue(response.cashTransactions[2].type == cashTransaction3.type, "The cash transaction type should be " + cashTransaction3.type + " but was " + response.cashTransactions[2].type);
		Assert.IsTrue(response.cashTransactions[3].type == cashTransaction4.type, "The cash transaction type should be " + cashTransaction4.type + " but was " + response.cashTransactions[3].type);
		Assert.IsTrue(response.cashTransactions[4].type == cashTransaction5.type, "The cash transaction type should be " + cashTransaction5.type + " but was " + response.cashTransactions[4].type);
	}

	[TestMethod]
	public void GetCashTransactionsTest_InvalidUserTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetCashTransactions.Endpoint("invalid"));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}
}