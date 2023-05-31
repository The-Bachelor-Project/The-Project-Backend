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
		transaction.price = new StockApp.Money(100, "USD");
		transaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction.AddToDb();
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 1, "There should be 1 portfolio but there were " + response.portfolios.Count);
		Assert.IsTrue(response.portfolios[0].stockTransactions.Count == 1, "There should be 1 transaction but there were " + response.portfolios[0].stockTransactions.Count);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].ticker == "TSLA", "Ticker should be TSLA but was " + response.portfolios[0].stockTransactions[0].ticker);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].exchange == "NASDAQ", "Exchange should be NASDAQ but was " + response.portfolios[0].stockTransactions[0].exchange);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].amount == 1, "Amount should be 1 but was " + response.portfolios[0].stockTransactions[0].amount);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].price!.amount == 100, "Price should be 100 but was " + response.portfolios[0].stockTransactions[0].price!.amount);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].price!.currency == "USD", "Currency should be USD but was " + response.portfolios[0].stockTransactions[0].price!.currency);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].timestamp == transaction.timestamp, "Timestamp should be " + transaction.timestamp + " but was " + response.portfolios[0].stockTransactions[0].timestamp);
	}

	[TestMethod]
	public async Task GetTransactionsTest_MultipleTransactionsSinglePortfolioTest()
	{
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio.id;
		transaction1.ticker = "TSLA";
		transaction1.exchange = "NASDAQ";
		transaction1.amount = 1;
		transaction1.price = new StockApp.Money(100, "USD");
		transaction1.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio.id;
		transaction2.ticker = "AAPL";
		transaction2.exchange = "NASDAQ";
		transaction2.amount = 1;
		transaction2.price = new StockApp.Money(100, "USD");
		transaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction2.AddToDb();
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 1, "There should be 1 portfolio but there were " + response.portfolios.Count);
		Assert.IsTrue(response.portfolios[0].stockTransactions.Count == 2, "There should be 2 transactions but there were " + response.portfolios[0].stockTransactions.Count);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].ticker == "TSLA", "Ticker should be TSLA but was " + response.portfolios[0].stockTransactions[0].ticker);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].exchange == "NASDAQ", "Exchange should be NASDAQ but was " + response.portfolios[0].stockTransactions[0].exchange);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].amount == 1, "Amount should be 1 but was " + response.portfolios[0].stockTransactions[0].amount);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].price!.amount == 100, "Price should be 100 but was " + response.portfolios[0].stockTransactions[0].price!.amount);
		Assert.IsTrue(response.portfolios[0].stockTransactions[0].price!.currency == "USD", "Currency should be USD but was " + response.portfolios[0].stockTransactions[0].price!.currency);
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
		transaction1.price = new StockApp.Money(100, "USD");
		transaction1.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio2.id;
		transaction2.ticker = "AAPL";
		transaction2.exchange = "NASDAQ";
		transaction2.amount = 1;
		transaction2.price = new StockApp.Money(100, "USD");
		transaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await transaction2.AddToDb();
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.portfolios.Count == 2, "There should be 2 portfolios but there were " + response.portfolios.Count);
		Assert.IsTrue(response.portfolios[0].stockTransactions.Count == 1, "There should be 1 transaction but there were " + response.portfolios[0].stockTransactions.Count);
		Assert.IsTrue(response.portfolios[1].stockTransactions.Count == 1, "There should be 1 transaction but there were " + response.portfolios[1].stockTransactions.Count);
	}

	[TestMethod]
	public void GetTransactionsTest_EmptyPortfolioTest()
	{
		GetTransactionsResponse response = GetTransactions.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
	}
}