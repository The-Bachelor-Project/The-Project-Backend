namespace BackendService.tests;

using StockApp;

[TestClass]
public class StockTransactionTest
{
	private UserTestObject userTestObject = null!;
	private Portfolio portfolio = null!;
	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
		portfolio = PortfolioHelper.Create(userTestObject);
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public async Task StockTransactionTest_AddToDb_SuccessfulTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await stockTransaction.AddToDb();
		Assert.IsTrue(stockTransaction.id != null, "ID was not set");
		Assert.IsTrue(stockTransaction.id != 0, "ID was not set");
	}

	[TestMethod]
	public async Task StockTransactionTest_AddToDb_InvalidTickerTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "invalid";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
		Assert.IsTrue(stockTransaction.id == null, "ID should not be set");
	}

	[TestMethod]
	public async Task StockTransactionTest_AddToDb_InvalidExchangeTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "invalid";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
		Assert.IsTrue(stockTransaction.id == null, "ID should not be set");
	}

	[TestMethod]
	public async Task StockTransactionTest_AddToDb_InvalidCurrencyTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "invalid");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
		Assert.IsTrue(stockTransaction.id == null, "ID should not be set");
	}

	[TestMethod]
	public async Task StockTransactionTest_AddToDb_NullElementsTest()
	{
		// Missing portfolio id
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransaction.portfolioId = null;
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 500 but was " + exception.StatusCode);

		// Missing ticker
		stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = null;
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 404 but was " + exception.StatusCode);

		// Missing exchange
		stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = null;
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 404 but was " + exception.StatusCode);

		// Missing amount
		stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = null;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 500 but was " + exception.StatusCode);

		// Missing timestamp
		stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = null;
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 500 but was " + exception.StatusCode);

		// Missing price_currency
		stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, null!);
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void StockTransactionTest_GetPortfolio_SuccessfulTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id;
		Portfolio gottenPortfolio = stockTransaction.GetPortfolio();
		Assert.IsTrue(gottenPortfolio.id == portfolio.id, "Portfolio ID should be " + portfolio.id + " but was " + gottenPortfolio.id);

	}

	[TestMethod]
	public void StockTransactionTest_GetPortfolio_InvalidIDTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => stockTransaction.GetPortfolio());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void StockTransactionTest_GetPortfolio_NullPortfolioIDTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => stockTransaction.GetPortfolio());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockTransactionTest_DeleteFromDb_SuccessfulTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransaction.portfolioId = portfolio.id;
		await stockTransaction.AddToDb();
		await stockTransaction.DeleteFromDb();
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => StockTransactionHelper.Get((int)stockTransaction.id!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task StockTransactionTest_DeleteFromDb_InvalidIDTest()
	{
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.price = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransaction.id = -1;
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await stockTransaction.DeleteFromDb());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}
}