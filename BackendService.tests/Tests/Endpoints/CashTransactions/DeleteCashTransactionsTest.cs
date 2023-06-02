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
	public void DeleteCashTransactionsTest_SuccessfulDeletionTest()
	{
		PostCashTransactionsBody body = new PostCashTransactionsBody(portfolio!.id!, "CAD", 100, Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")), "BUY", "TEST");

		// DeleteCashTransactionsResponse response = DeleteCashTransactions.Endpoint()
	}

	[TestMethod]
	public void DeleteCashTransactionsTest_InvalidPortfolioTest()
	{

	}

	[TestMethod]
	public void DeleteCashTransactionsTest_WrongPortoflioTest()
	{

	}

	[TestMethod]
	public void DeleteCashTransactionsTest_WrongUserTest()
	{

	}

	[TestMethod]
	public void DeleteCashTransactionsTest_NonExistigCashTransactionTest()
	{

	}

	[TestMethod]
	public void DeleteCashTransactionsTest_MissingValuesTest()
	{

	}

	[TestMethod]
	public void DeleteCashTransactionsTest_NullValuesTest()
	{

	}

	[TestMethod]
	public void DeleteCashTransactionsTest_DeletetionOfWrongTypeTest()
	{

	}


}