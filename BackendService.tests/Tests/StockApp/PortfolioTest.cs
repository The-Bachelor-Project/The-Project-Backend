namespace BackendService.tests;
using StockApp;

[TestClass]
public class PortfolioTest
{
	private UserTestObject userTestObject = null!;
	private Portfolio portfolio = null!;
	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
		portfolio = new Portfolio(
			"test",
			userTestObject.user!.id!,
			"USD",
			true
		);
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public void PortfolioTest_AddToDb_SuccessfulTest()
	{
		portfolio.AddToDb();
		Assert.IsTrue(portfolio.id != null, "Portfolio ID was not set");
		Portfolio gottenPortfolio = PortfolioHelper.Get(portfolio.id!);
		Assert.IsTrue(gottenPortfolio.name == portfolio.name, "Portfolio name should be \"" + portfolio.name + "\" but was \"" + gottenPortfolio.name + "\"");
		Assert.IsTrue(gottenPortfolio.owner == portfolio.owner, "Portfolio owner should be \"" + portfolio.owner + "\" but was \"" + gottenPortfolio.owner + "\"");
		Assert.IsTrue(gottenPortfolio.currency == portfolio.currency, "Portfolio currency should be \"" + portfolio.currency + "\" but was \"" + gottenPortfolio.currency + "\"");
	}

	[TestMethod]
	public void PortfolioTest_AddToDb_InvalidUserIDTest()
	{
		portfolio.owner = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.AddToDb());
		Assert.IsTrue(exception.StatusCode == 500, "Status code should be 500 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_AddToDb_NullElementsTest()
	{
		// Portfolio name
		Portfolio portfolio = new Portfolio(
			null!,
			userTestObject.user!.id!,
			"USD",
			true
		);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Portfolio owner
		portfolio = new Portfolio(
			"test",
			null!,
			"USD",
			true
		);
		exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Portfolio currency
		portfolio = new Portfolio(
			"test",
			userTestObject.user!.id!,
			null!,
			true
		);
		exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

	}

	[TestMethod]
	public void PortfolioTest_AddToDb_InvalidCurrencyTest()
	{
		portfolio.currency = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.AddToDb());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_GetOwner_SuccessfulTest()
	{
		portfolio.AddToDb();
		User gottenUser = portfolio.GetOwner();
		Assert.IsTrue(gottenUser.id == userTestObject.user!.id, "User ID should be \"" + userTestObject.user!.id + "\" but was \"" + gottenUser.id + "\"");
	}

	[TestMethod]
	public void PortfolioTest_GetOwner_InvalidPortfolioIDTest()
	{
		portfolio.AddToDb();
		portfolio.id = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.GetOwner());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_GetOwner_NullPortfolioIDTest()
	{
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.GetOwner());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_UpdateStockTransactions_SingleTransactionTest()
	{
		portfolio.AddToDb();
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "TSLA";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.amount = 10;
		stockTransaction.priceNative = new Money(100, "USD");
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await stockTransaction.AddToDb();
		portfolio.UpdateStockTransactions();
		Assert.IsTrue(portfolio.stockTransactions.Count == 1, "Stock transaction count should be 1 but was " + portfolio.stockTransactions.Count);
		Assert.IsTrue(portfolio.stockTransactions[0].id == stockTransaction.id, "Stock transaction ID should be \"" + stockTransaction.id + "\" but was \"" + portfolio.stockTransactions[0].id + "\"");

	}

	[TestMethod]
	public async Task PortfolioTest_UpdateStockTransactions_MultipleTransactionsTest()
	{
		portfolio.AddToDb();
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "TSLA";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.amount = 10;
		stockTransaction.priceNative = new Money(100, "USD");
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await stockTransaction.AddToDb();
		int tempID = (int)stockTransaction.id!;
		await stockTransaction.AddToDb();
		portfolio.UpdateStockTransactions();
		Assert.IsTrue(portfolio.stockTransactions.Count == 2, "Stock transaction count should be 2 but was " + portfolio.stockTransactions.Count);
		Assert.IsTrue(portfolio.stockTransactions[0].id == tempID, "Stock transaction ID should be \"" + tempID + "\" but was \"" + portfolio.stockTransactions[0].id + "\"");
		Assert.IsTrue(portfolio.stockTransactions[1].id == stockTransaction.id, "Stock transaction ID should be \"" + stockTransaction.id + "\" but was \"" + portfolio.stockTransactions[1].id + "\"");
	}

	[TestMethod]
	public void PortfolioTest_UpdateStockTransactions_EmptyTransactionTest()
	{
		portfolio.AddToDb();
		portfolio.UpdateStockTransactions();
		Assert.IsTrue(portfolio.stockTransactions.Count == 0, "Stock transaction count should be 0 but was " + portfolio.stockTransactions.Count);
	}

	[TestMethod]
	public void PortfolioTest_UpdateStockTransactions_NullPortfolioIDTest()
	{
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.UpdateStockTransactions());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_UpdateStockPositions_SinglePositionTest()
	{
		portfolio.AddToDb();
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "TSLA";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.amount = 10;
		stockTransaction.priceNative = new Money(100, "USD");
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await stockTransaction.AddToDb();
		portfolio.UpdateStockPositions();
		Assert.IsTrue(portfolio.stockPositions.Count == 1, "Stock position count should be 1 but was " + portfolio.stockPositions.Count);
		Assert.IsTrue(portfolio.stockPositions[0].stock.ticker == stockTransaction.ticker, "Stock position ticker should be \"" + stockTransaction.ticker + "\" but was \"" + portfolio.stockPositions[0].stock.ticker + "\"");
	}

	[TestMethod]
	public async Task PortfolioTest_UpdateStockPositions_MultiplePositionsTest()
	{
		portfolio.AddToDb();
		StockTransaction stockTransaction1 = new StockTransaction();
		stockTransaction1.portfolioId = portfolio.id!;
		stockTransaction1.ticker = "TSLA";
		stockTransaction1.exchange = "NASDAQ";
		stockTransaction1.amount = 10;
		stockTransaction1.priceNative = new Money(100, "USD");
		stockTransaction1.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await stockTransaction1.AddToDb();
		StockTransaction stockTransaction2 = new StockTransaction();
		stockTransaction2.portfolioId = portfolio.id!;
		stockTransaction2.ticker = "CHEMM";
		stockTransaction2.exchange = "CPH";
		stockTransaction2.amount = 10;
		stockTransaction2.priceNative = new Money(100, "DKK");
		stockTransaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await stockTransaction2.AddToDb();
		portfolio.UpdateStockPositions();
		Assert.IsTrue(portfolio.stockPositions.Count == 2, "Stock position count should be 2 but was " + portfolio.stockPositions.Count);
		Assert.IsTrue(portfolio.stockPositions[0].stock.ticker == stockTransaction2.ticker, "Stock position ticker should be \"" + stockTransaction1.ticker + "\" but was \"" + portfolio.stockPositions[0].stock.ticker + "\"");
		Assert.IsTrue(portfolio.stockPositions[1].stock.ticker == stockTransaction1.ticker, "Stock position ticker should be \"" + stockTransaction2.ticker + "\" but was \"" + portfolio.stockPositions[1].stock.ticker + "\"");
	}

	[TestMethod]
	public void PortfolioTest_UpdateStockPositions_EmptyPositionTest()
	{
		portfolio.AddToDb();
		portfolio.UpdateStockPositions();
		Assert.IsTrue(portfolio.stockPositions.Count == 0, "Stock position count should be 0 but was " + portfolio.stockPositions.Count);
	}

	[TestMethod]
	public void PortfolioTest_UpdateStockPositions_NullPortfolioIDTest()
	{
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.UpdateStockPositions());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_GetValueHistory_SinglePortfolioSingleTransactionTest()
	{
		portfolio.AddToDb();
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "TSLA";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.amount = 10;
		stockTransaction.priceNative = new Money(100, "USD");
		stockTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2021-04-04"));
		await stockTransaction.AddToDb();
		await portfolio.GetValueHistory("USD", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-06-06"));
		Assert.IsTrue(portfolio.stockPositions.Count == 1, "Stock position count should be 1 but was " + portfolio.stockPositions.Count);
		Assert.IsTrue(portfolio.stockPositions[0].stock.ticker == stockTransaction.ticker, "Stock position ticker should be \"" + stockTransaction.ticker + "\" but was \"" + portfolio.stockPositions[0].stock.ticker + "\"");
		Assert.IsTrue(portfolio.stockTransactions.Count == 1, "Stock transaction count should be 1 but was " + portfolio.stockTransactions.Count);
		Assert.IsTrue(portfolio.stockTransactions[0].ticker == stockTransaction.ticker, "Stock transaction ticker should be \"" + stockTransaction.ticker + "\" but was \"" + portfolio.stockTransactions[0].ticker + "\"");
		Assert.IsTrue(portfolio.valueHistory!.Count > 0, "Value history count should be bigger than 0 but was " + portfolio.valueHistory.Count);
	}

	[TestMethod]
	public async Task PortfolioTest_GetValueHistory_SinglePortfolioMultipleTransactionsTest()
	{
		portfolio.AddToDb();
		StockTransaction stockTransaction1 = new StockTransaction();
		stockTransaction1.portfolioId = portfolio.id!;
		stockTransaction1.ticker = "TSLA";
		stockTransaction1.exchange = "NASDAQ";
		stockTransaction1.amount = 10;
		stockTransaction1.priceNative = new Money(100, "USD");
		stockTransaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2021-04-04"));
		await stockTransaction1.AddToDb();
		StockTransaction stockTransaction2 = new StockTransaction();
		stockTransaction2.portfolioId = portfolio.id!;
		stockTransaction2.ticker = "CHEMM";
		stockTransaction2.exchange = "CPH";
		stockTransaction2.amount = 10;
		stockTransaction2.priceNative = new Money(100, "DKK");
		stockTransaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2021-04-04"));
		await stockTransaction2.AddToDb();
		await portfolio.GetValueHistory("USD", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-06-06"));
		Assert.IsTrue(portfolio.stockPositions.Count == 2, "Stock position count should be 2 but was " + portfolio.stockPositions.Count);
		Assert.IsTrue(portfolio.stockPositions[0].stock.ticker == stockTransaction2.ticker, "Stock position ticker should be \"" + stockTransaction1.ticker + "\" but was \"" + portfolio.stockPositions[0].stock.ticker + "\"");
		Assert.IsTrue(portfolio.stockPositions[1].stock.ticker == stockTransaction1.ticker, "Stock position ticker should be \"" + stockTransaction2.ticker + "\" but was \"" + portfolio.stockPositions[1].stock.ticker + "\"");
		Assert.IsTrue(portfolio.stockTransactions.Count == 2, "Stock transaction count should be 2 but was " + portfolio.stockTransactions.Count);
		Assert.IsTrue(portfolio.stockTransactions[0].ticker == stockTransaction1.ticker, "Stock transaction ticker should be \"" + stockTransaction1.ticker + "\" but was \"" + portfolio.stockTransactions[0].ticker + "\"");
		Assert.IsTrue(portfolio.stockTransactions[1].ticker == stockTransaction2.ticker, "Stock transaction ticker should be \"" + stockTransaction2.ticker + "\" but was \"" + portfolio.stockTransactions[1].ticker + "\"");
		Assert.IsTrue(portfolio.valueHistory!.Count > 0, "Value history count should be bigger than 0 but was " + portfolio.valueHistory.Count);
	}

	[TestMethod]
	public async Task PortfolioTest_GetValueHistory_MultiplePortfoliosSingleTransactionsTest()
	{
		portfolio.AddToDb();
		Portfolio portfolio2 = new Portfolio(
			"Test",
			userTestObject.user!.id!,
			"DKK",
			true
		);
		portfolio2.AddToDb();
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "TSLA";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.amount = 10;
		stockTransaction.priceNative = new Money(100, "USD");
		stockTransaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2021-04-04"));
		await stockTransaction.AddToDb();
		await portfolio.GetValueHistory("USD", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-06-06"));
		await portfolio2.GetValueHistory("USD", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-06-06"));
		Assert.IsTrue(portfolio.stockPositions.Count == 1, "Stock position count should be 1 but was " + portfolio.stockPositions.Count);
		Assert.IsTrue(portfolio.stockPositions[0].stock.ticker == stockTransaction.ticker, "Stock position ticker should be \"" + stockTransaction.ticker + "\" but was \"" + portfolio.stockPositions[0].stock.ticker + "\"");
		Assert.IsTrue(portfolio.stockTransactions.Count == 1, "Stock transaction count should be 1 but was " + portfolio.stockTransactions.Count);
		Assert.IsTrue(portfolio.stockTransactions[0].ticker == stockTransaction.ticker, "Stock transaction ticker should be \"" + stockTransaction.ticker + "\" but was \"" + portfolio.stockTransactions[0].ticker + "\"");
		Assert.IsTrue(portfolio.valueHistory!.Count > 0, "Value history count should be bigger than 0 but was " + portfolio.valueHistory.Count);
		Assert.IsTrue(portfolio2.stockPositions.Count == 0, "Stock position count should be 0 but was " + portfolio2.stockPositions.Count);
		Assert.IsTrue(portfolio2.stockTransactions.Count == 0, "Stock transaction count should be 0 but was " + portfolio2.stockTransactions.Count);
		Assert.IsTrue(portfolio2.valueHistory!.Count == 0, "Value history count should be 0 but was " + portfolio2.valueHistory.Count);
	}

	[TestMethod]
	public async Task PortfolioTest_GetValueHistory_InvalidCurrencyTest()
	{
		portfolio.AddToDb();
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await portfolio.GetValueHistory("invalid", DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-01-02")));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_GetValueHistory_InvalidDatesTest()
	{
		portfolio.AddToDb();
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await portfolio.GetValueHistory("USD", DateOnly.Parse("2021-10-10"), DateOnly.Parse("2021-01-01")));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_GetValueHistory_CurrencyNullTest()
	{
		portfolio.AddToDb();
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await portfolio.GetValueHistory(null!, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2021-01-02")));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_ChangeName_SuccessfulTest()
	{
		portfolio.AddToDb();
		portfolio.ChangeName("New name");
		Portfolio gottenPortfolio = PortfolioHelper.Get(portfolio.id!);
		Assert.IsTrue(gottenPortfolio.name == "New name", "Portfolio name should be \"New name\" but was \"" + gottenPortfolio.name + "\"");
	}

	[TestMethod]
	public void PortfolioTest_ChangeName_NullPortfolioIDTest()
	{
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.ChangeName("New name"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_ChangeName_NullNameTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.ChangeName(null));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_ChangeCurrency_SuccessfulTest()
	{
		portfolio.AddToDb();
		portfolio.ChangeCurrency("DKK");
		Portfolio gottenPortfolio = PortfolioHelper.Get(portfolio.id!);
		Assert.IsTrue(gottenPortfolio.currency == "DKK", "Portfolio currency should be \"DKK\" but was \"" + gottenPortfolio.currency + "\"");
	}

	[TestMethod]
	public void PortfolioTest_ChangeCurrency_NullPortfolioIDTest()
	{
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.ChangeCurrency("DKK"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_ChangeCurrency_NullCurrencyTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.ChangeCurrency(null));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_ChangeCurrency_InvalidCurrencyTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.ChangeCurrency("Invalid currency"));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_GetStockTransaction_SuccessfulTest()
	{
		portfolio.AddToDb();
		StockTransaction stockTransaction = new StockTransaction();
		stockTransaction.portfolioId = portfolio.id!;
		stockTransaction.ticker = "AAPL";
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.priceNative = new Money(100, "USD");
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		await stockTransaction.AddToDb();
		StockTransaction gottenStockTransaction = portfolio.GetStockTransaction((int)stockTransaction.id!);
		Assert.IsTrue(gottenStockTransaction.id == stockTransaction.id, "Stock transaction ID should be " + stockTransaction.id + " but was " + gottenStockTransaction.id);
		Assert.IsTrue(gottenStockTransaction.portfolioId == stockTransaction.portfolioId, "Stock transaction portfolio ID should be " + stockTransaction.portfolioId + " but was " + gottenStockTransaction.portfolioId);
	}

	[TestMethod]
	public void PortfolioTest_GetStockTransaction_NullPortfolioIDTest()
	{
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.GetStockTransaction(1));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_Delete_SuccessfulTest()
	{
		portfolio.AddToDb();
		portfolio.Delete();
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetPortfolios.Endpoint(portfolio.id!, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_Delete_InvalidPortfolioIDTest()
	{
		portfolio.id = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.Delete());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_Delete_NullPortfolioIDTest()
	{
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.Delete());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_GetCashTransaction_SuccessfulTest()
	{
		portfolio.AddToDb();
		CashTransaction cashTransaction = new CashTransaction();
		cashTransaction.portfolioId = portfolio.id!;
		cashTransaction.nativeAmount = new Money(100, "USD");
		cashTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction.description = "Test";
		await cashTransaction.AddToDb();
		CashTransaction gottenCashTransaction = portfolio.GetCashTransaction((int)cashTransaction.id!);
		Assert.IsTrue(gottenCashTransaction.id == cashTransaction.id, "Cash transaction ID should be " + cashTransaction.id + " but was " + gottenCashTransaction.id);
		Assert.IsTrue(gottenCashTransaction.portfolioId == cashTransaction.portfolioId, "Cash transaction portfolio ID should be " + cashTransaction.portfolioId + " but was " + gottenCashTransaction.portfolioId);
	}

	[TestMethod]
	public async Task PortfolioTest_GetCashTransaction_PortfolioIDNullTest()
	{
		portfolio.AddToDb();
		CashTransaction cashTransaction = new CashTransaction();
		cashTransaction.portfolioId = portfolio.id!;
		cashTransaction.nativeAmount = new Money(100, "USD");
		cashTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction.description = "Test";
		await cashTransaction.AddToDb();
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.GetCashTransaction((int)cashTransaction.id!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_GetCashTransaction_NonExistentTest()
	{
		portfolio.AddToDb();
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.GetCashTransaction(-123456789));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task PortfolioTest_UpdateCashTransactions_SuccessfulTest()
	{
		portfolio.AddToDb();
		CashTransaction cashTransaction = new CashTransaction();
		cashTransaction.portfolioId = portfolio.id!;
		cashTransaction.nativeAmount = new Money(100, "USD");
		cashTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction.description = "Test";
		await cashTransaction.AddToDb();
		CashTransaction cashTransaction2 = new CashTransaction();
		cashTransaction2.portfolioId = portfolio.id!;
		cashTransaction2.nativeAmount = new Money(100, "USD");
		cashTransaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction2.description = "Test";
		await cashTransaction2.AddToDb();
		portfolio.UpdateCashTransactions();
		Assert.IsTrue(portfolio.cashTransactions.Count == 2, "Cash transaction count should be 2 but was " + portfolio.cashTransactions.Count);
		Assert.IsTrue(portfolio.cashTransactions[0].id == cashTransaction.id, "Cash transaction ID should be " + cashTransaction.id + " but was " + portfolio.cashTransactions[0].id);
		Assert.IsTrue(portfolio.cashTransactions[1].id == cashTransaction2.id, "Cash transaction ID should be " + cashTransaction2.id + " but was " + portfolio.cashTransactions[1].id);
	}

	[TestMethod]
	public async Task PortfolioTest_UpdateCashTransactions_PortfolioIDNullTest()
	{
		portfolio.AddToDb();
		CashTransaction cashTransaction = new CashTransaction();
		cashTransaction.portfolioId = portfolio.id!;
		cashTransaction.nativeAmount = new Money(100, "USD");
		cashTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction.description = "Test";
		await cashTransaction.AddToDb();
		CashTransaction cashTransaction2 = new CashTransaction();
		cashTransaction2.portfolioId = portfolio.id!;
		cashTransaction2.nativeAmount = new Money(100, "USD");
		cashTransaction2.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		cashTransaction2.description = "Test";
		await cashTransaction2.AddToDb();
		portfolio.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.UpdateCashTransactions());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PortfolioTest_UpdateCashTransactions_EmptyCashTransactionListTest()
	{
		portfolio.AddToDb();
		portfolio.UpdateCashTransactions();
		Assert.IsTrue(portfolio.cashTransactions.Count == 0, "Cash transaction count should be 0 but was " + portfolio.cashTransactions.Count);
	}


}