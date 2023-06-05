namespace BackendService.tests;

[TestClass]
public class GetTransactionsTest
{
	private static UserTestObject userTestObject = new UserTestObject();
	private static StockApp.Portfolio portfolio = null!;

	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
		portfolio = PortfolioHelper.Create(userTestObject);
	}

	[TestCleanup]
	public void Cleanup()
	{
		PortfolioHelper.Delete(userTestObject);
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public async Task GetTransactionsTest_SingleTransactionSinglePortfolioTest()
	{
		StockApp.StockTransaction transaction = new StockApp.StockTransaction();
		transaction.portfolioId = portfolio.id;
		transaction.ticker = "TSLA";
		transaction.exchange = "NASDAQ";
		transaction.amount = 1;
		transaction.priceNative = new StockApp.Money(100, "USD");
		transaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction.AddToDb();
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.transactions.Count == 1, "There should be 1 transaction but there were " + response.transactions.Count);
		Assert.IsTrue(response.transactions[0].description.Contains("TSLA"), "Description should contain TSLA but was " + response.transactions[0].description);
		Assert.IsTrue(response.transactions[0].description.Contains("NASDAQ"), "Description should contain NASDAQ but was " + response.transactions[0].description);
		Assert.IsTrue(response.transactions[0].value.amount == 100, "Amount should be 100 but was " + response.transactions[0].value.amount);
		Assert.IsTrue(response.transactions[0].value.currency == "USD", "Currency should be USD but was " + response.transactions[0].value.currency);
		Assert.IsTrue(response.transactions[0].balance.amount == 100, "Balance should be 100 but was " + response.transactions[0].balance.amount);
		Assert.IsTrue(response.transactions[0].balance.currency == "USD", "Currency should be USD but was " + response.transactions[0].balance.currency);
		Assert.IsTrue(response.transactions[0].combinedBalance.amount == 100, "Combined balance should be 100 but was " + response.transactions[0].combinedBalance.amount);
		Assert.IsTrue(response.transactions[0].combinedBalance.currency == "USD", "Currency should be USD but was " + response.transactions[0].combinedBalance.currency);
	}

	[TestMethod]
	public async Task GetTransactionsTest_MultipleTransactionsSinglePortfolioTest()
	{
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio.id;
		transaction1.ticker = "TSLA";
		transaction1.exchange = "NASDAQ";
		transaction1.amount = 1;
		transaction1.priceNative = new StockApp.Money(-100, "USD");
		transaction1.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio.id;
		transaction2.ticker = "AAPL";
		transaction2.exchange = "NASDAQ";
		transaction2.amount = 1;
		transaction2.priceNative = new StockApp.Money(-100, "USD");
		transaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction2.AddToDb();
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);

		Assert.IsTrue(response.transactions[0].description.Contains("TSLA"), "Description should contain TSLA but was " + response.transactions[0].description);
		Assert.IsTrue(response.transactions[0].description.Contains("NASDAQ"), "Description should contain NASDAQ but was " + response.transactions[0].description);
		Assert.IsTrue(response.transactions[0].value.amount == -100, "Amount should be 100 but was " + response.transactions[0].value.amount);
		Assert.IsTrue(response.transactions[0].value.currency == "USD", "Currency should be USD but was " + response.transactions[0].value.currency);
		Assert.IsTrue(response.transactions[0].balance.amount == -100, "Balance should be -100 but was " + response.transactions[0].balance.amount);
		Assert.IsTrue(response.transactions[0].balance.currency == "USD", "Currency should be USD but was " + response.transactions[0].balance.currency);
		Assert.IsTrue(response.transactions[0].combinedBalance.amount == -100, "Combined balance should be -100 but was " + response.transactions[0].combinedBalance.amount);
		Assert.IsTrue(response.transactions[0].combinedBalance.currency == "USD", "Currency should be USD but was " + response.transactions[0].combinedBalance.currency);

		Assert.IsTrue(response.transactions[1].description.Contains("AAPL"), "Description should contain AAPL but was " + response.transactions[1].description);
		Assert.IsTrue(response.transactions[1].description.Contains("NASDAQ"), "Description should contain NASDAQ but was " + response.transactions[1].description);
		Assert.IsTrue(response.transactions[1].value.amount == -100, "Amount should be 100 but was " + response.transactions[1].value.amount);
		Assert.IsTrue(response.transactions[1].value.currency == "USD", "Currency should be USD but was " + response.transactions[1].value.currency);
		Assert.IsTrue(response.transactions[1].balance.amount == -200, "Balance should be -200 but was " + response.transactions[1].balance.amount);
		Assert.IsTrue(response.transactions[1].balance.currency == "USD", "Currency should be USD but was " + response.transactions[1].balance.currency);
		Assert.IsTrue(response.transactions[1].combinedBalance.amount == -200, "Combined balance should be -200 but was " + response.transactions[1].combinedBalance.amount);
		Assert.IsTrue(response.transactions[1].combinedBalance.currency == "USD", "Currency should be USD but was " + response.transactions[1].combinedBalance.currency);
	}

	[TestMethod]
	public async Task GetTransactionsTest_MultipleTransactionsMultiplePortfoliosTest()
	{
		StockApp.Portfolio portfolio2 = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio.id;
		transaction1.ticker = "TSLA";
		transaction1.exchange = "NASDAQ";
		transaction1.amount = 1;
		transaction1.priceNative = new StockApp.Money(-100, "USD");
		transaction1.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio2.id;
		transaction2.ticker = "AAPL";
		transaction2.exchange = "NASDAQ";
		transaction2.amount = 1;
		transaction2.priceNative = new StockApp.Money(-100, "USD");
		transaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction2.AddToDb();
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.transactions[0].description.Contains("TSLA"), "Description should contain TSLA but was " + response.transactions[0].description);
		Assert.IsTrue(response.transactions[0].description.Contains("NASDAQ"), "Description should contain NASDAQ but was " + response.transactions[0].description);
		Assert.IsTrue(response.transactions[0].value.amount == -100, "Amount should be 100 but was " + response.transactions[0].value.amount);
		Assert.IsTrue(response.transactions[0].value.currency == "USD", "Currency should be USD but was " + response.transactions[0].value.currency);
		Assert.IsTrue(response.transactions[0].balance.amount == -100, "Balance should be -100 but was " + response.transactions[0].balance.amount);
		Assert.IsTrue(response.transactions[0].balance.currency == "USD", "Currency should be USD but was " + response.transactions[0].balance.currency);
		Assert.IsTrue(response.transactions[0].combinedBalance.amount == -100, "Combined balance should be -100 but was " + response.transactions[0].combinedBalance.amount);
		Assert.IsTrue(response.transactions[0].combinedBalance.currency == "USD", "Currency should be USD but was " + response.transactions[0].combinedBalance.currency);

		Assert.IsTrue(response.transactions[1].description.Contains("AAPL"), "Description should contain AAPL but was " + response.transactions[1].description);
		Assert.IsTrue(response.transactions[1].description.Contains("NASDAQ"), "Description should contain NASDAQ but was " + response.transactions[1].description);
		Assert.IsTrue(response.transactions[1].value.amount == -100, "Amount should be 100 but was " + response.transactions[1].value.amount);
		Assert.IsTrue(response.transactions[1].value.currency == "USD", "Currency should be USD but was " + response.transactions[1].value.currency);
		Assert.IsTrue(response.transactions[1].balance.amount == -100, "Balance should be -100 but was " + response.transactions[1].balance.amount);
		Assert.IsTrue(response.transactions[1].balance.currency == "USD", "Currency should be USD but was " + response.transactions[1].balance.currency);
		Assert.IsTrue(response.transactions[1].combinedBalance.amount == -200, "Combined balance should be -200 but was " + response.transactions[1].combinedBalance.amount);
		Assert.IsTrue(response.transactions[1].combinedBalance.currency == "USD", "Currency should be USD but was " + response.transactions[1].combinedBalance.currency);

	}

	[TestMethod]
	public void GetTransactionsTest_EmptyPortfolioTest()
	{
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.transactions.Count == 0, "Transactions should be empty but was " + response.transactions.Count);
	}
}