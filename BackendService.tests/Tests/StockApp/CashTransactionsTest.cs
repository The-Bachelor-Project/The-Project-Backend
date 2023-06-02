namespace BackendService.tests;

[TestClass]
public class CashTransactionsTest
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
	public async Task CashTransactionsTest_AddToDb_SingleTransactionTest()
	{
		StockApp.CashTransaction cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction.type = "BUY";
		cashTransaction.description = "TEST";
		cashTransaction = await cashTransaction.AddToDb();
		StockApp.CashTransaction cashTransactionFromDb = CashTransactionHelper.Get((int)cashTransaction.id!);
		Assert.IsTrue(cashTransactionFromDb.id == cashTransaction.id, "Cash transaction id should be " + cashTransaction.id + " but was " + cashTransactionFromDb.id);
		Assert.IsTrue(cashTransactionFromDb.portfolioId == cashTransaction.portfolioId, "Cash transaction portfolio id should be " + cashTransaction.portfolioId + " but was " + cashTransactionFromDb.portfolioId);
		Assert.IsTrue(cashTransactionFromDb.nativeAmount!.amount == cashTransaction.nativeAmount!.amount, "Cash transaction native amount should be " + cashTransaction.nativeAmount.amount + " but was " + cashTransactionFromDb.nativeAmount.amount);
		Assert.IsTrue(cashTransactionFromDb.nativeAmount.currency == cashTransaction.nativeAmount.currency, "Cash transaction native amount currency should be " + cashTransaction.nativeAmount.currency + " but was " + cashTransactionFromDb.nativeAmount.currency);
		Assert.IsTrue(cashTransactionFromDb.timestamp == cashTransaction.timestamp, "Cash transaction timestamp should be " + cashTransaction.timestamp + " but was " + cashTransactionFromDb.timestamp);
		Assert.IsTrue(cashTransactionFromDb.type == cashTransaction.type, "Cash transaction type should be " + cashTransaction.type + " but was " + cashTransactionFromDb.type);
		Assert.IsTrue(cashTransactionFromDb.description == cashTransaction.description, "Cash transaction description should be " + cashTransaction.description + " but was " + cashTransactionFromDb.description);
	}

	[TestMethod]
	public async Task CashTransactionsTest_AddToDb_MultipleTransactionsTest()
	{
		StockApp.CashTransaction cashTransaction1 = new StockApp.CashTransaction();
		cashTransaction1.portfolioId = portfolio!.id;
		cashTransaction1.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction1.type = "BUY";
		cashTransaction1.description = "TEST";
		cashTransaction1 = await cashTransaction1.AddToDb();
		StockApp.CashTransaction cashTransaction2 = new StockApp.CashTransaction();
		cashTransaction2.portfolioId = portfolio!.id;
		cashTransaction2.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction2.type = "BUY";
		cashTransaction2.description = "TEST";
		cashTransaction2 = await cashTransaction2.AddToDb();
		StockApp.CashTransaction cashTransaction3 = new StockApp.CashTransaction();
		cashTransaction3.portfolioId = portfolio!.id;
		cashTransaction3.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction3.type = "BUY";
		cashTransaction3.description = "TEST";
		cashTransaction3 = await cashTransaction3.AddToDb();
		StockApp.CashTransaction cashTransaction4 = new StockApp.CashTransaction();
		cashTransaction4.portfolioId = portfolio!.id;
		cashTransaction4.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction4.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction4.type = "BUY";
		cashTransaction4.description = "TEST";
		cashTransaction4 = await cashTransaction4.AddToDb();

		StockApp.CashTransaction cashTransactionFromDb1 = CashTransactionHelper.Get((int)cashTransaction1.id!);
		StockApp.CashTransaction cashTransactionFromDb2 = CashTransactionHelper.Get((int)cashTransaction2.id!);
		StockApp.CashTransaction cashTransactionFromDb3 = CashTransactionHelper.Get((int)cashTransaction3.id!);
		StockApp.CashTransaction cashTransactionFromDb4 = CashTransactionHelper.Get((int)cashTransaction4.id!);
		Assert.IsTrue(cashTransactionFromDb1.id == cashTransaction1.id, "Cash transaction id should be " + cashTransaction1.id + " but was " + cashTransactionFromDb1.id);
		Assert.IsTrue(cashTransactionFromDb2.id == cashTransaction2.id, "Cash transaction id should be " + cashTransaction2.id + " but was " + cashTransactionFromDb2.id);
		Assert.IsTrue(cashTransactionFromDb3.id == cashTransaction3.id, "Cash transaction id should be " + cashTransaction3.id + " but was " + cashTransactionFromDb3.id);
		Assert.IsTrue(cashTransactionFromDb4.id == cashTransaction4.id, "Cash transaction id should be " + cashTransaction4.id + " but was " + cashTransactionFromDb4.id);
	}

	[TestMethod]
	public async Task CashTransactionsTest_AddToDb_InvalidCurrencyTest()
	{
		StockApp.CashTransaction cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.nativeAmount = new StockApp.Money(100, "invalid");
		cashTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction.type = "BUY";
		cashTransaction.description = "TEST";
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await cashTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task CashTransactionsTest_AddToDb_NullFieldsTest()
	{
		// Portfolio id
		StockApp.CashTransaction cashTransaction = new StockApp.CashTransaction();
		cashTransaction.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction.type = "BUY";
		cashTransaction.description = "TEST";
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await cashTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Native amount
		cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction.type = "BUY";
		cashTransaction.description = "TEST";
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await cashTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Native amount currency
		cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.nativeAmount = new StockApp.Money(100, null!);
		cashTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction.type = "BUY";
		cashTransaction.description = "TEST";
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await cashTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Timestamp
		cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction.type = "BUY";
		cashTransaction.description = "TEST";
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await cashTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Type
		cashTransaction = new StockApp.CashTransaction();
		cashTransaction.portfolioId = portfolio!.id;
		cashTransaction.nativeAmount = new StockApp.Money(100, "JPY");
		cashTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		cashTransaction.description = "TEST";
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await cashTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

}