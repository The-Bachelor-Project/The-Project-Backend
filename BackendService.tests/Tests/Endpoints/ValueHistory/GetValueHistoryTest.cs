namespace BackendService.tests;

[TestClass]
public class GetValueHistoryTest
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
	public async Task GetValueHistoryTest_SingleTransactionWithoutDividendTest()
	{
		StockApp.StockTransaction transaction = new StockApp.StockTransaction();
		transaction.portfolioId = portfolio.id;
		transaction.amount = 10;
		transaction.price = new StockApp.Money(100, "DKK");
		transaction.ticker = "TSLA";
		transaction.exchange = "NASDAQ";
		transaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		await transaction.AddToDb();
		GetValueHistoryResponse response = await GetValueHistory.EndpointAsync("2020-01-01", "2021-01-01", "USD", userTestObject.accessToken!);
		Assert.IsTrue(response.valueHistory.portfolios.Count == 1, "There should be one portfolio in the response, but there was " + response.valueHistory.portfolios.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions.Count == 1, "There should be one position in the response, but there was " + response.valueHistory.portfolios[0].positions.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].ticker == "TSLA", "The ticker should be TSLA but was " + response.valueHistory.portfolios[0].positions[0].ticker);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].exchange == "NASDAQ", "The exchange should be NASDAQ but was " + response.valueHistory.portfolios[0].positions[0].exchange);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].valueHistory.Count > 0, "There should be more than 0 value history entries, but there was " + response.valueHistory.portfolios[0].positions[0].valueHistory.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].dividends.Count == 0, "There should be 0 dividends, but there was " + response.valueHistory.portfolios[0].positions[0].dividends.Count);
		await transaction.DeleteFromDb();
	}

	[TestMethod]
	public async Task GetValueHistoryTest_SingleTransactionWithDividendTest()
	{
		StockApp.StockTransaction transaction = new StockApp.StockTransaction();
		transaction.portfolioId = portfolio.id;
		transaction.amount = 10;
		transaction.price = new StockApp.Money(100, "DKK");
		transaction.ticker = "VICI";
		transaction.exchange = "NYSE";
		transaction.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		await transaction.AddToDb();
		GetValueHistoryResponse response = await GetValueHistory.EndpointAsync("2020-01-01", "2021-01-01", "USD", userTestObject.accessToken!);
		Assert.IsTrue(response.valueHistory.portfolios.Count == 1, "There should be one portfolio in the response, but there was " + response.valueHistory.portfolios.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions.Count == 1, "There should be one position in the response, but there was " + response.valueHistory.portfolios[0].positions.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].ticker == "VICI", "The ticker should be VICI but was " + response.valueHistory.portfolios[0].positions[0].ticker);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].exchange == "NYSE", "The exchange should be NYSE but was " + response.valueHistory.portfolios[0].positions[0].exchange);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].valueHistory.Count > 0, "There should be more than 0 value history entries, but there was " + response.valueHistory.portfolios[0].positions[0].valueHistory.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions[0].dividends.Count > 0, "There should be more than 0 dividends, but there was " + response.valueHistory.portfolios[0].positions[0].dividends.Count);
	}

	[TestMethod]
	public async Task GetValueHistoryTest_MultipleTransactionsWithoutDividendTest()
	{
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio.id;
		transaction1.amount = 10;
		transaction1.price = new StockApp.Money(100, "DKK");
		transaction1.ticker = "TSLA";
		transaction1.exchange = "NASDAQ";
		transaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio.id;
		transaction2.amount = 20;
		transaction2.price = new StockApp.Money(200, "DKK");
		transaction2.ticker = "GOOGL";
		transaction2.exchange = "NASDAQ";
		transaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		await transaction2.AddToDb();
		StockApp.StockTransaction transaction3 = new StockApp.StockTransaction();
		transaction3.portfolioId = portfolio.id;
		transaction3.amount = 30;
		transaction3.price = new StockApp.Money(300, "DKK");
		transaction3.ticker = "DIS";
		transaction3.exchange = "NYSE";
		transaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		await transaction3.AddToDb();
		GetValueHistoryResponse response = await GetValueHistory.EndpointAsync("2020-01-01", "2021-01-01", "USD", userTestObject.accessToken!);
		Assert.IsTrue(response.valueHistory.portfolios.Count == 1, "There should be one portfolio in the response, but there was " + response.valueHistory.portfolios.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions.Count == 3, "There should be three positions in the response, but there was " + response.valueHistory.portfolios[0].positions.Count);
		for (int i = 0; i < response.valueHistory.portfolios[0].positions.Count; i++)
		{
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].valueHistory.Count > 0, "There should be more than 0 value history entries, but there was " + response.valueHistory.portfolios[0].positions[i].valueHistory.Count);
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].dividends.Count == 0, "There should be 0 dividends, but there was " + response.valueHistory.portfolios[0].positions[i].dividends.Count);
		}
	}

	[TestMethod]
	public async Task GetValueHistoryTest_MultipleTransactionsWithDividendTest()
	{
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio.id;
		transaction1.amount = 10;
		transaction1.price = new StockApp.Money(100, "EUR");
		transaction1.ticker = "BGS";
		transaction1.exchange = "NYSE";
		transaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio.id;
		transaction2.amount = 20;
		transaction2.price = new StockApp.Money(200, "USD");
		transaction2.ticker = "VICI";
		transaction2.exchange = "NYSE";
		transaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		await transaction2.AddToDb();
		StockApp.StockTransaction transaction3 = new StockApp.StockTransaction();
		transaction3.portfolioId = portfolio.id;
		transaction3.amount = 30;
		transaction3.price = new StockApp.Money(300, "CAD");
		transaction3.ticker = "T";
		transaction3.exchange = "NYSE";
		transaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		await transaction3.AddToDb();
		GetValueHistoryResponse response = await GetValueHistory.EndpointAsync("2020-01-01", "2021-01-01", "USD", userTestObject.accessToken!);
		Assert.IsTrue(response.valueHistory.portfolios.Count == 1, "There should be one portfolio in the response, but there was " + response.valueHistory.portfolios.Count);
		Assert.IsTrue(response.valueHistory.portfolios[0].positions.Count == 3, "There should be three positions in the response, but there was " + response.valueHistory.portfolios[0].positions.Count);
		for (int i = 0; i < response.valueHistory.portfolios[0].positions.Count; i++)
		{
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].valueHistory.Count > 0, "There should be more than 0 value history entries, but there was " + response.valueHistory.portfolios[0].positions[i].valueHistory.Count);
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].dividends.Count > 0, "There should be more than 0 dividends, but there was " + response.valueHistory.portfolios[0].positions[i].dividends.Count);
		}
	}

	[TestMethod]
	public async Task GetValueHistoryTest_MultiplePortfoliosWithoutDividendTest()
	{
		StockApp.Portfolio portfolio1 = PortfolioHelper.Create(userTestObject);
		StockApp.Portfolio portfolio2 = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio1.id;
		transaction1.amount = 10;
		transaction1.price = new StockApp.Money(100, "EUR");
		transaction1.ticker = "TSLA";
		transaction1.exchange = "NASDAQ";
		transaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio2.id;
		transaction2.amount = 20;
		transaction2.price = new StockApp.Money(200, "USD");
		transaction2.ticker = "GOOGL";
		transaction2.exchange = "NASDAQ";
		transaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		await transaction2.AddToDb();
		StockApp.StockTransaction transaction3 = new StockApp.StockTransaction();
		transaction3.portfolioId = portfolio2.id;
		transaction3.amount = 30;
		transaction3.price = new StockApp.Money(300, "CAD");
		transaction3.ticker = "DIS";
		transaction3.exchange = "NYSE";
		transaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		await transaction3.AddToDb();
		GetValueHistoryResponse response = await GetValueHistory.EndpointAsync("2020-01-01", "2021-01-01", "USD", userTestObject.accessToken!);
		Assert.IsTrue(response.valueHistory.portfolios.Count == 3, "There should be two portfolios in the response, but there was " + response.valueHistory.portfolios.Count);
		for (int i = 0; i < response.valueHistory.portfolios[0].positions.Count; i++)
		{
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].valueHistory.Count > 0, "There should be more than 0 value history entries, but there was " + response.valueHistory.portfolios[0].positions[i].valueHistory.Count);
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].dividends.Count == 0, "There should be 0 dividends, but there was " + response.valueHistory.portfolios[0].positions[i].dividends.Count);
		}
	}

	[TestMethod]
	public async Task GetValueHistoryTest_MultiplePortfoliosWithDividendTest()
	{
		StockApp.Portfolio portfolio1 = PortfolioHelper.Create(userTestObject);
		StockApp.Portfolio portfolio2 = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio1.id;
		transaction1.amount = 10;
		transaction1.price = new StockApp.Money(100, "EUR");
		transaction1.ticker = "T";
		transaction1.exchange = "NYSE";
		transaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio2.id;
		transaction2.amount = 20;
		transaction2.price = new StockApp.Money(200, "USD");
		transaction2.ticker = "BGS";
		transaction2.exchange = "NYSE";
		transaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		await transaction2.AddToDb();
		StockApp.StockTransaction transaction3 = new StockApp.StockTransaction();
		transaction3.portfolioId = portfolio2.id;
		transaction3.amount = 30;
		transaction3.price = new StockApp.Money(300, "CAD");
		transaction3.ticker = "VICI";
		transaction3.exchange = "NYSE";
		transaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		await transaction3.AddToDb();
		GetValueHistoryResponse response = await GetValueHistory.EndpointAsync("2020-01-01", "2021-01-01", "USD", userTestObject.accessToken!);
		Assert.IsTrue(response.valueHistory.portfolios.Count == 3, "There should be two portfolios in the response, but there was " + response.valueHistory.portfolios.Count);
		for (int i = 0; i < response.valueHistory.portfolios[0].positions.Count; i++)
		{
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].valueHistory.Count > 0, "There should be more than 0 value history entries, but there was " + response.valueHistory.portfolios[0].positions[i].valueHistory.Count);
			Assert.IsTrue(response.valueHistory.portfolios[0].positions[i].dividends.Count > 0, "There should be more than 0 dividends, but there was " + response.valueHistory.portfolios[0].positions[i].dividends.Count);
		}
	}

	[TestMethod]
	public async Task GetValueHistoryTest_InvalidDatesTest()
	{
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetValueHistory.EndpointAsync("2021-01-01", "2020-01-01", "USD", userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "The status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task GetValueHistoryTest_AllCurrenciesTest()
	{
		StockApp.Portfolio portfolio1 = PortfolioHelper.Create(userTestObject);
		StockApp.Portfolio portfolio2 = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction transaction1 = new StockApp.StockTransaction();
		transaction1.portfolioId = portfolio1.id;
		transaction1.amount = 10;
		transaction1.price = new StockApp.Money(100, "EUR");
		transaction1.ticker = "T";
		transaction1.exchange = "NYSE";
		transaction1.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-10-10"));
		await transaction1.AddToDb();
		StockApp.StockTransaction transaction2 = new StockApp.StockTransaction();
		transaction2.portfolioId = portfolio2.id;
		transaction2.amount = 20;
		transaction2.price = new StockApp.Money(200, "USD");
		transaction2.ticker = "BGS";
		transaction2.exchange = "NYSE";
		transaction2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-08-08"));
		await transaction2.AddToDb();
		StockApp.StockTransaction transaction3 = new StockApp.StockTransaction();
		transaction3.portfolioId = portfolio2.id;
		transaction3.amount = 30;
		transaction3.price = new StockApp.Money(300, "CAD");
		transaction3.ticker = "VICI";
		transaction3.exchange = "NYSE";
		transaction3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2020-06-06"));
		await transaction3.AddToDb();
		GetValueHistoryResponse response;
		foreach (String currency in Dictionaries.currencies)
		{
			response = await GetValueHistory.EndpointAsync("2020-01-01", "2021-01-01", currency, userTestObject.accessToken!);
			Assert.IsTrue(response.valueHistory.portfolios.Count == 3, "There should be two portfolios in the response, but there was " + response.valueHistory.portfolios.Count);
			Assert.IsTrue(response.valueHistory.valueHistory[0].highPrice.currency == currency, "The currency of the high price should be " + currency + " but was " + response.valueHistory.valueHistory[0].highPrice.currency);
		}
	}
}