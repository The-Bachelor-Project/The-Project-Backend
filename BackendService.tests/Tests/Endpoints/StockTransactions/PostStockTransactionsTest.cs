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
		successfulStockTransaction.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction1.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction2.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction3.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction1.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction2.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction3.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction1.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction2.priceNative = new StockApp.Money(100, "USD");
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
		stockTransaction3.priceNative = new StockApp.Money(100, "USD");
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
	public async Task PostStockTransactionsTest_MissingPortfolioIDTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_MissingElementsTestTest()
	{
		// Missing portfolio id
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing portfolio id should be 400 but was " + exception.StatusCode);

		// Missing exchange
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing exchange should be 400 but was " + exception.StatusCode);

		// Missing ticker
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing ticker should be 400 but was " + exception.StatusCode);

		// Missing timestamp
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing timestamp should be 400 but was " + exception.StatusCode);

		// Missing amount
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing amount should be 400 but was " + exception.StatusCode);

		// Missing price currency
		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.priceNative = new StockApp.Money(100, null!);
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code for missing price currency should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_InvalidAccessTokenTest()
	{
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(successfulStockTransaction);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, "invalid"));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_InvalidPortfolioOwnerTest()
	{
		UserTestObject invalidUser = UserHelper.Create();
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(successfulStockTransaction);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, invalidUser.accessToken!));
		Assert.IsTrue(exception.StatusCode == 403, "Status code should be 403 but was " + exception.StatusCode);
		UserHelper.Delete(invalidUser);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_InvalidCurrencyTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "invalid");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_InvalidTickerTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "invalid";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_InvalidExchangeTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "invalid";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_InvalidTickerAndExchangeTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "invalid";
		stockTransactionData.ticker = "invalid";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_InvalidPriceTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(-100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_NullCurrencyTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, null!);
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_MinusAmountOwnedTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);

		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = -2;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "AAPL";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = -1;
		stockTransactionData.exchange = "NASDAQ";
		stockTransactionData.ticker = "TSLA";
		stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}



	[TestMethod]
	public async Task PostStockTransactionsTest_AllCurrenciesConversionTest()
	{
		foreach (String currency in Dictionaries.currencies)
		{
			StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
			stockTransactionData.portfolioId = portfolio.id;
			stockTransactionData.amount = 1;
			stockTransactionData.exchange = "NASDAQ";
			stockTransactionData.ticker = "AAPL";
			stockTransactionData.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
			stockTransactionData.priceNative = new StockApp.Money(100, currency);
			PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
			PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			StockApp.StockTransaction gottenStockTransaction = StockTransactionHelper.Get(response.id!);
			Assert.IsTrue(gottenStockTransaction.portfolioId == stockTransactionData.portfolioId, "Portfolio id should be " + stockTransactionData.portfolioId + " but was " + gottenStockTransaction.portfolioId);
			Assert.IsTrue(gottenStockTransaction.priceNative!.amount == stockTransactionData.priceNative.amount, "Price should be " + stockTransactionData.priceNative.amount + " but was " + gottenStockTransaction.priceNative.amount);
			Assert.IsTrue(gottenStockTransaction.priceNative.currency == stockTransactionData.priceNative.currency, "Currency should be " + stockTransactionData.priceNative.currency + " but was " + gottenStockTransaction.priceNative.currency);
			Assert.IsTrue(gottenStockTransaction.priceUSD!.amount != stockTransactionData.priceNative.amount, "Price should not be " + stockTransactionData.priceNative.amount + " but was " + gottenStockTransaction.priceUSD.amount);
			Assert.IsTrue(gottenStockTransaction.priceUSD.currency == "USD", "Currency should be USD but was " + gottenStockTransaction.priceUSD.currency);
		}
	}

	[TestMethod]
	public async Task PostStockTransactionsTest_DividendPayoutTests_SuccessfulCreatedTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 10;
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.ticker = "O";
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(stockTransactionData);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.StockTransaction gottenStockTransaction = StockTransactionHelper.Get(response.id!);
		GetTransactionsResponse transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions.portfolios[0].dividendPayouts.Count);
	}
}