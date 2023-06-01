namespace BackendService.tests;

[TestClass]
public class PostStockTransactionsTest
{
	private static UserTestObject userTestObject = new UserTestObject();
	private static StockApp.Portfolio portfolio = null!;
	private static StockApp.StockTransaction successfulStockTransaction = null!;

	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();

		portfolio = PortfolioHelper.Create(userTestObject);
		successfulStockTransaction = new StockApp.StockTransaction();
		successfulStockTransaction.portfolioId = portfolio.id;
		successfulStockTransaction.amount = 1;
		successfulStockTransaction.exchange = "NASDAQ";
		successfulStockTransaction.ticker = "AAPL";
		successfulStockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		successfulStockTransaction.price = new StockApp.Money(100, "USD");
	}

	[TestCleanup]
	public void Cleanup()
	{
		PortfolioHelper.Delete(userTestObject);
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_PostSuccessfullyTest()
	{
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(successfulStockTransaction);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_PostAscendingOrderTest()
	{
		StockApp.StockTransaction stockTransaction1 = new StockApp.StockTransaction();
		stockTransaction1.portfolioId = portfolio.id;
		stockTransaction1.amount = 10;
		stockTransaction1.exchange = "NASDAQ";
		stockTransaction1.ticker = "AAPL";
		stockTransaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		stockTransaction1.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction1);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction1ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		StockApp.StockTransaction gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 10, "Amount owned for first transaction should be 10 but was " + gottenStockTransaction.amountOwned);

		StockApp.StockTransaction stockTransaction2 = new StockApp.StockTransaction();
		stockTransaction2.portfolioId = portfolio.id;
		stockTransaction2.amount = 20;
		stockTransaction2.exchange = "NASDAQ";
		stockTransaction2.ticker = "AAPL";
		stockTransaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		stockTransaction2.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction2);
		response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction2ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction2ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 30, "Amount owned for the first transaction should be 30 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 10, "Amount owned for the second transaction should be 10 but was " + gottenStockTransaction.amountOwned);


		StockApp.StockTransaction stockTransaction3 = new StockApp.StockTransaction();
		stockTransaction3.portfolioId = portfolio.id;
		stockTransaction3.amount = 30;
		stockTransaction3.exchange = "NASDAQ";
		stockTransaction3.ticker = "AAPL";
		stockTransaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		stockTransaction3.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction3);
		response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction3ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction3ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 60, "Amount owned for the third transaction should be 60 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction2ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 30, "Amount owned for the second transaction should be 30 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 10, "Amount owned for the first transaction should be 10 but was " + gottenStockTransaction.amountOwned);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_PostDescendingOrderTest()
	{
		StockApp.StockTransaction stockTransaction1 = new StockApp.StockTransaction();
		stockTransaction1.portfolioId = portfolio.id;
		stockTransaction1.amount = 10;
		stockTransaction1.exchange = "NASDAQ";
		stockTransaction1.ticker = "AAPL";
		stockTransaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		stockTransaction1.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction1);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction1ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		StockApp.StockTransaction gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 10, "Amount owned for first transaction should be 10 but was " + gottenStockTransaction.amountOwned);

		StockApp.StockTransaction stockTransaction2 = new StockApp.StockTransaction();
		stockTransaction2.portfolioId = portfolio.id;
		stockTransaction2.amount = 20;
		stockTransaction2.exchange = "NASDAQ";
		stockTransaction2.ticker = "AAPL";
		stockTransaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		stockTransaction2.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction2);
		response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction2ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction2ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 20, "Amount owned for the first transaction should be 20 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 30, "Amount owned for the second transaction should be 30 but was " + gottenStockTransaction.amountOwned);


		StockApp.StockTransaction stockTransaction3 = new StockApp.StockTransaction();
		stockTransaction3.portfolioId = portfolio.id;
		stockTransaction3.amount = 30;
		stockTransaction3.exchange = "NASDAQ";
		stockTransaction3.ticker = "AAPL";
		stockTransaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		stockTransaction3.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction3);
		response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction3ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction3ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 30, "Amount owned for the third transaction should be 30 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction2ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 50, "Amount owned for the second transaction should be 50 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 60, "Amount owned for the first transaction should be 60 but was " + gottenStockTransaction.amountOwned);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_PostAscAndDescOrderTest()
	{
		StockApp.StockTransaction stockTransaction1 = new StockApp.StockTransaction();
		stockTransaction1.portfolioId = portfolio.id;
		stockTransaction1.amount = 10;
		stockTransaction1.exchange = "NASDAQ";
		stockTransaction1.ticker = "AAPL";
		stockTransaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		stockTransaction1.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction1);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction1ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		StockApp.StockTransaction gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 10, "Amount owned for first transaction should be 10 but was " + gottenStockTransaction.amountOwned);

		StockApp.StockTransaction stockTransaction2 = new StockApp.StockTransaction();
		stockTransaction2.portfolioId = portfolio.id;
		stockTransaction2.amount = 20;
		stockTransaction2.exchange = "NASDAQ";
		stockTransaction2.ticker = "AAPL";
		stockTransaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		stockTransaction2.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction2);
		response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction2ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction2ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 20, "Amount owned for the first transaction should be 20 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 30, "Amount owned for the second transaction should be 30 but was " + gottenStockTransaction.amountOwned);


		StockApp.StockTransaction stockTransaction3 = new StockApp.StockTransaction();
		stockTransaction3.portfolioId = portfolio.id;
		stockTransaction3.amount = 30;
		stockTransaction3.exchange = "NASDAQ";
		stockTransaction3.ticker = "AAPL";
		stockTransaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		stockTransaction3.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransaction3);
		response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		int stockTransaction3ID = response.id;
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != 0, "Response id should not be 0");
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction3ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 60, "Amount owned for the third transaction should be 60 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction2ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 20, "Amount owned for the second transaction should be 20 but was " + gottenStockTransaction.amountOwned);
		gottenStockTransaction = StockTransactionHelper.Get(stockTransaction1ID);
		Assert.IsTrue(gottenStockTransaction.amountOwned == 30, "Amount owned for the first transaction should be 30 but was " + gottenStockTransaction.amountOwned);
	}

	[TestMethod]
	public void PostStockTransactionsTest_MissingPortfolioIDTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_MissingElementsTestTest()
	{
		// Missing portfolio id
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing portfolio id should be 400 but was " + exception.StatusCode);

		// Missing exchange
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing exchange should be 400 but was " + exception.StatusCode);

		// Missing ticker
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing ticker should be 400 but was " + exception.StatusCode);

		// Missing timestamp
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing timestamp should be 400 but was " + exception.StatusCode);

		// Missing amount
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.price = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing amount should be 400 but was " + exception.StatusCode);

		// Missing price currency
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.price = new StockApp.Money(100, null!);
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing price currency should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_InvalidAccessTokenTest()
	{
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(successfulStockTransaction);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, "invalid").GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_InvalidPortfolioOwnerTest()
	{
		UserTestObject invalidUser = UserHelper.Create();
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(successfulStockTransaction);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, invalidUser.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 403, "Status code should be 403 but was " + exception.StatusCode);
		UserHelper.Delete(invalidUser);
	}

	[TestMethod]
	public void PostStockTransactionsTest_InvalidCurrencyTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "invalid");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_InvalidTickerTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "invalid";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_InvalidExchangeTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "invalid";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_InvalidTickerAndExchangeTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "invalid";
		stockTransactionData.ticker = "invalid";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_InvalidPriceTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(-100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostStockTransactionsTest_NullCurrencyTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.price = new StockApp.Money(100, null!);
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

}