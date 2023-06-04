namespace BackendService.tests;

[TestClass]
public class TransactionsTest
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
	public async Task TransactionsTest_DividendPayoutTests_SellingAndBuyingTest()
	{

		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 10;
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.ticker = "O";
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-06-06"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		GetTransactionsResponse transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions.portfolios[0].dividendPayouts.Count);
		StockApp.StockTransaction stockTransactionData2 = new StockApp.StockTransaction();
		stockTransactionData2.portfolioId = portfolio.id;
		stockTransactionData2.amount = -10;
		stockTransactionData2.exchange = "NYSE";
		stockTransactionData2.ticker = "O";
		stockTransactionData2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01"));
		stockTransactionData2.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody2 = new PostStockTransactionsBody(
			stockTransactionData2.portfolioId!,
			stockTransactionData2.ticker,
			stockTransactionData2.exchange,
			stockTransactionData2.amount,
			stockTransactionData2.timestamp,
			stockTransactionData2.priceNative
		);
		PostStockTransactionsResponse response2 = await PostStockTransactions.EndpointAsync(postStockTransactionsBody2, userTestObject.accessToken!);
		Assert.IsTrue(response2.response == "success", "Response should be success but was " + response2.response);
		transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions.portfolios[0].dividendPayouts.Count);

		StockApp.StockTransaction stockTransactionData3 = new StockApp.StockTransaction();
		stockTransactionData3.portfolioId = portfolio.id;
		stockTransactionData3.amount = 10;
		stockTransactionData3.exchange = "NYSE";
		stockTransactionData3.ticker = "O";
		stockTransactionData3.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-03-03"));
		stockTransactionData3.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody3 = new PostStockTransactionsBody(
			stockTransactionData3.portfolioId!,
			stockTransactionData3.ticker,
			stockTransactionData3.exchange,
			stockTransactionData3.amount,
			stockTransactionData3.timestamp,
			stockTransactionData3.priceNative
		);
		PostStockTransactionsResponse response3 = await PostStockTransactions.EndpointAsync(postStockTransactionsBody3, userTestObject.accessToken!);
		Assert.IsTrue(response3.response == "success", "Response should be success but was " + response3.response);
		transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions.portfolios[0].dividendPayouts.Count);
		StockApp.StockTransaction stockTransactionData4 = new StockApp.StockTransaction();
		stockTransactionData4.portfolioId = portfolio.id;
		stockTransactionData4.amount = -10;
		stockTransactionData4.exchange = "NYSE";
		stockTransactionData4.ticker = "O";
		stockTransactionData4.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-05-05"));
		stockTransactionData4.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody4 = new PostStockTransactionsBody(
			stockTransactionData4.portfolioId!,
			stockTransactionData4.ticker,
			stockTransactionData4.exchange,
			stockTransactionData4.amount,
			stockTransactionData4.timestamp,
			stockTransactionData4.priceNative
		);
		PostStockTransactionsResponse response4 = await PostStockTransactions.EndpointAsync(postStockTransactionsBody4, userTestObject.accessToken!);
		Assert.IsTrue(response4.response == "success", "Response should be success but was " + response4.response);
		transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions.portfolios[0].dividendPayouts.Count);
		// TODO: Fix this, please

		// foreach (StockApp.DividendPayout payout in transactions.portfolios[0].dividendPayouts)
		// {
		// 	DateOnly timestamp = Tools.TimeConverter.UnixTimeStampToDateOnly(payout.timestamp);
		// 	if (timestamp.Month == 2)
		// 	{
		// 		Assert.Fail("There should be no dividend payout in February but there was one");
		// 	}
		// }
	}

	[TestMethod]
	public async Task TransactionsTest_DividendPayoutTests_ChangeOriginalBoughtAmountTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 10;
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.ticker = "O";
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-06-06"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		PostStockTransactionsResponse response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.StockTransaction gottenStockTransaction1 = StockTransactionHelper.Get(response.id!);
		StockApp.StockTransaction stockTransactionData2 = new StockApp.StockTransaction();
		stockTransactionData2.portfolioId = portfolio.id;
		stockTransactionData2.amount = -10;
		stockTransactionData2.exchange = "NYSE";
		stockTransactionData2.ticker = "O";
		stockTransactionData2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08"));
		stockTransactionData2.priceNative = new StockApp.Money(100, "USD");
		postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData2.portfolioId!,
			stockTransactionData2.ticker,
			stockTransactionData2.exchange,
			stockTransactionData2.amount,
			stockTransactionData2.timestamp,
			stockTransactionData2.priceNative
		);
		response = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.StockTransaction gottenStockTransaction2 = StockTransactionHelper.Get(response.id!);
		Assert.IsTrue(gottenStockTransaction1.amountOwned == 10, "Amount owned should be 10 but was " + gottenStockTransaction1.amountOwned);
		Assert.IsTrue(gottenStockTransaction2.amountOwned == 0, "Amount owned should be 0 but was " + gottenStockTransaction2.amountOwned);
		GetTransactionsResponse transactions1 = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions1.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions1.portfolios[0].dividendPayouts.Count);
		PutStockTransactionsBody putBody = new PutStockTransactionsBody(
			(int)gottenStockTransaction1.id!,
			gottenStockTransaction1.portfolioId!,
			15,
			0,
			0,
			""
		);
		PutStockTransactionsResponse putResponse = await PutStockTransactions.Endpoint(userTestObject.accessToken!, putBody);
		Assert.IsTrue(putResponse.response == "success", "Response should be success but was " + putResponse.response);
		gottenStockTransaction1 = StockTransactionHelper.Get(response.id!);
		Assert.IsTrue(gottenStockTransaction1.amountOwned == 15, "Amount owned should be 15 but was " + gottenStockTransaction1.amountOwned);
		GetTransactionsResponse transactions2 = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions2.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions2.portfolios[0].dividendPayouts.Count);
		Assert.IsTrue(transactions2.portfolios[0].dividendPayouts.Count > transactions1.portfolios[0].dividendPayouts.Count, "Dividend payouts should be more than " + transactions1.portfolios[0].dividendPayouts.Count + " but was " + transactions2.portfolios[0].dividendPayouts.Count);

		putBody = new PutStockTransactionsBody(
			(int)gottenStockTransaction1.id!,
			gottenStockTransaction1.portfolioId!,
			10,
			Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-04-04")),
			0,
			""
		);
		putResponse = await PutStockTransactions.Endpoint(userTestObject.accessToken!, putBody);
		Assert.IsTrue(putResponse.response == "success", "Response should be success but was " + putResponse.response);
		gottenStockTransaction1 = StockTransactionHelper.Get(response.id!);
		Assert.IsTrue(gottenStockTransaction1.amountOwned == 10, "Amount owned should be 10 but was " + gottenStockTransaction1.amountOwned);
		GetTransactionsResponse transactions3 = GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions3.portfolios[0].dividendPayouts.Count > 0, "Dividend payouts should be more than 0 but was " + transactions3.portfolios[0].dividendPayouts.Count);
		Assert.IsTrue(transactions3.portfolios[0].dividendPayouts.Count > transactions1.portfolios[0].dividendPayouts.Count && transactions3.portfolios[0].dividendPayouts.Count < transactions2.portfolios[0].dividendPayouts.Count, "Dividend payouts should be more than " + transactions1.portfolios[0].dividendPayouts.Count + " but was " + transactions3.portfolios[0].dividendPayouts.Count);
	}

	[TestMethod]
	public async Task TransactionsTest_BalanceTest_PositiveTransactionsTest()
	{
		StockApp.CashTransaction cashTransactionData = new StockApp.CashTransaction();
		cashTransactionData.portfolioId = portfolio.id;
		cashTransactionData.nativeAmount = new StockApp.Money(200, "USD");
		cashTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08"));
		cashTransactionData.description = "Test";
		PostCashTransactionsBody postCashTransactionsBody = new PostCashTransactionsBody(
			cashTransactionData.portfolioId!,
			cashTransactionData.nativeAmount.currency,
			cashTransactionData.nativeAmount.amount,
			(int)cashTransactionData.timestamp,
			cashTransactionData.description
		);
		PostCashTransactionsResponse postCashTransactionsResponse = await PostCashTransactions.Endpoint(postCashTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postCashTransactionsResponse.response == "success", "Response should be success but was " + postCashTransactionsResponse.response);

		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 10;
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.ticker = "O";
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-09"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse.response == "success", "Response should be success but was " + postStockTransactionsResponse.response);

		StockApp.StockTransaction stockTransactionData2 = new StockApp.StockTransaction();
		stockTransactionData2.portfolioId = portfolio.id;
		stockTransactionData2.amount = 10;
		stockTransactionData2.exchange = "NYSE";
		stockTransactionData2.ticker = "O";
		stockTransactionData2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-11-11"));
		stockTransactionData2.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody2 = new PostStockTransactionsBody(
			stockTransactionData2.portfolioId!,
			stockTransactionData2.ticker,
			stockTransactionData2.exchange,
			stockTransactionData2.amount,
			stockTransactionData2.timestamp,
			stockTransactionData2.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse2 = await PostStockTransactions.EndpointAsync(postStockTransactionsBody2, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse2.response == "success", "Response should be success but was " + postStockTransactionsResponse2.response);

		GetTransactionsResponse transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		List<TransactionTestObject> transactionsList = TransactionHelper.CombinedTransactions(transactions);
		Assert.IsTrue(transactionsList[0].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08")), "Timestamp should be 2022-08-08 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[0].timestamp).ToString());
		Assert.IsTrue(transactionsList[0].balanceAmount == 200, "Balance should be 200 but was " + transactionsList[0].balanceAmount);
		Assert.IsTrue(transactionsList[1].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-09")), "Timestamp should be 2022-08-09 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[1].timestamp).ToString());
		Assert.IsTrue(transactionsList[1].balanceAmount == 100, "Balance should be 100 but was " + transactionsList[1].balanceAmount);
		Assert.IsTrue(transactionsList[2].balanceAmount > 100, "Balance should be bigger than 100 but was " + transactionsList[2].balanceAmount);
		Assert.IsTrue(transactionsList[3].balanceAmount > transactionsList[2].balanceAmount, "Balance should be bigger than " + transactionsList[2].balanceAmount + " but was " + transactionsList[3].balanceAmount);
		Assert.IsTrue(transactionsList[4].balanceAmount > transactionsList[3].balanceAmount, "Balance should be bigger than " + transactionsList[3].balanceAmount + " but was " + transactionsList[4].balanceAmount);
		Assert.IsTrue(transactionsList[5].balanceAmount > transactionsList[4].balanceAmount, "Balance should be bigger than " + transactionsList[4].balanceAmount + " but was " + transactionsList[5].balanceAmount);
		Assert.IsTrue(transactionsList[6].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-11-11")), "Timestamp should be 2022-11-11 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[6].timestamp).ToString());
		Assert.IsTrue(transactionsList[6].balanceAmount == transactionsList[5].balanceAmount - 100, "Balance should be equal " + (transactionsList[5].balanceAmount - 100) + " but was " + transactionsList[6]);
	}

	[TestMethod]
	public async Task TransactionsTest_BalanceTest_PositiveAndNegativeTransactionsTest()
	{
		StockApp.CashTransaction cashTransactionData = new StockApp.CashTransaction();
		cashTransactionData.portfolioId = portfolio.id;
		cashTransactionData.nativeAmount = new StockApp.Money(200, "USD");
		cashTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08"));
		cashTransactionData.description = "Test";
		PostCashTransactionsBody postCashTransactionsBody = new PostCashTransactionsBody(
			cashTransactionData.portfolioId!,
			cashTransactionData.nativeAmount.currency,
			cashTransactionData.nativeAmount.amount,
			(int)cashTransactionData.timestamp,
			cashTransactionData.description
		);
		PostCashTransactionsResponse postCashTransactionsResponse = await PostCashTransactions.Endpoint(postCashTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postCashTransactionsResponse.response == "success", "Response should be success but was " + postCashTransactionsResponse.response);

		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 10;
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.ticker = "O";
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse.response == "success", "Response should be success but was " + postStockTransactionsResponse.response);

		StockApp.StockTransaction stockTransactionData2 = new StockApp.StockTransaction();
		stockTransactionData2.portfolioId = portfolio.id;
		stockTransactionData2.amount = -10;
		stockTransactionData2.exchange = "NYSE";
		stockTransactionData2.ticker = "O";
		stockTransactionData2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-09-09"));
		stockTransactionData2.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody2 = new PostStockTransactionsBody(
			stockTransactionData2.portfolioId!,
			stockTransactionData2.ticker,
			stockTransactionData2.exchange,
			stockTransactionData2.amount,
			stockTransactionData2.timestamp,
			stockTransactionData2.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse2 = await PostStockTransactions.EndpointAsync(postStockTransactionsBody2, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse2.response == "success", "Response should be success but was " + postStockTransactionsResponse2.response);

		StockApp.CashTransaction cashTransactionData2 = new StockApp.CashTransaction();
		cashTransactionData2.portfolioId = portfolio.id;
		cashTransactionData2.nativeAmount = new StockApp.Money(-50, "USD");
		cashTransactionData2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-10"));
		cashTransactionData2.description = "Test";
		PostCashTransactionsBody postCashTransactionsBody2 = new PostCashTransactionsBody(
			cashTransactionData2.portfolioId!,
			cashTransactionData2.nativeAmount.currency,
			cashTransactionData2.nativeAmount.amount,
			(int)cashTransactionData2.timestamp,
			cashTransactionData2.description
		);
		PostCashTransactionsResponse postCashTransactionsResponse3 = await PostCashTransactions.Endpoint(postCashTransactionsBody2, userTestObject.accessToken!);
		Assert.IsTrue(postCashTransactionsResponse3.response == "success", "Response should be success but was " + postCashTransactionsResponse3.response);

		GetTransactionsResponse transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		List<TransactionTestObject> transactionsList = TransactionHelper.CombinedTransactions(transactions);
		Assert.IsTrue(transactionsList[0].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08")), "Timestamp should be 2022-08-08 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[0].timestamp).ToString());
		Assert.IsTrue(transactionsList[0].balanceAmount == 200, "Balance should be 200 but was " + transactionsList[0].balanceAmount);
		Assert.IsTrue(transactionsList[1].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08")), "Timestamp should be 2022-08-08 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[1].timestamp).ToString());
		Assert.IsTrue(transactionsList[1].balanceAmount == 100, "Balance should be 100 but was " + transactionsList[1].balanceAmount);
		Assert.IsTrue(transactionsList[2].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-10")), "Timestamp should be 2022-08-10 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[2].timestamp).ToString());
		Assert.IsTrue(transactionsList[2].balanceAmount == 50, "Balance should be 50 but was " + transactionsList[2].balanceAmount);
		Assert.IsTrue(transactionsList[3].balanceAmount > transactionsList[2].balanceAmount, "Balance should be bigger than " + transactionsList[2].balanceAmount + " but was " + transactionsList[3].balanceAmount);
		Assert.IsTrue(transactionsList[4].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-09-09")), "Timestamp should be 2022-09-09 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[4].timestamp).ToString());
		Assert.IsTrue(transactionsList[4].balanceAmount == (transactionsList[3].balanceAmount + 100), "Balance should be " + (transactionsList[3].balanceAmount + 100) + " but was " + transactionsList[4].balanceAmount);
	}

	[TestMethod]
	public async Task TransactionsTest_BalanceTest_ChangeTransactionsTest()
	{
		StockApp.CashTransaction cashTransactionData = new StockApp.CashTransaction();
		cashTransactionData.portfolioId = portfolio.id;
		cashTransactionData.nativeAmount = new StockApp.Money(200, "USD");
		cashTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08"));
		cashTransactionData.description = "Test";
		PostCashTransactionsBody postCashTransactionsBody = new PostCashTransactionsBody(
			cashTransactionData.portfolioId!,
			cashTransactionData.nativeAmount.currency,
			cashTransactionData.nativeAmount.amount,
			(int)cashTransactionData.timestamp,
			cashTransactionData.description
		);
		PostCashTransactionsResponse postCashTransactionsResponse = await PostCashTransactions.Endpoint(postCashTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postCashTransactionsResponse.response == "success", "Response should be success but was " + postCashTransactionsResponse.response);

		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.amount = 10;
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.ticker = "O";
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-09"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse = await PostStockTransactions.EndpointAsync(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse.response == "success", "Response should be success but was " + postStockTransactionsResponse.response);

		StockApp.StockTransaction stockTransactionData2 = new StockApp.StockTransaction();
		stockTransactionData2.portfolioId = portfolio.id;
		stockTransactionData2.amount = 10;
		stockTransactionData2.exchange = "NYSE";
		stockTransactionData2.ticker = "O";
		stockTransactionData2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-10-20"));
		stockTransactionData2.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody2 = new PostStockTransactionsBody(
			stockTransactionData2.portfolioId!,
			stockTransactionData2.ticker,
			stockTransactionData2.exchange,
			stockTransactionData2.amount,
			stockTransactionData2.timestamp,
			stockTransactionData2.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse2 = await PostStockTransactions.EndpointAsync(postStockTransactionsBody2, userTestObject.accessToken!);

		StockApp.CashTransaction cashTransactionData2 = new StockApp.CashTransaction();
		cashTransactionData2.portfolioId = portfolio.id;
		cashTransactionData2.nativeAmount = new StockApp.Money(-50, "USD");
		cashTransactionData2.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-09-09"));
		cashTransactionData2.description = "Test";
		PostCashTransactionsBody postCashTransactionsBody2 = new PostCashTransactionsBody(
			cashTransactionData2.portfolioId!,
			cashTransactionData2.nativeAmount.currency,
			cashTransactionData2.nativeAmount.amount,
			(int)cashTransactionData2.timestamp,
			cashTransactionData2.description
		);
		PostCashTransactionsResponse postCashTransactionsResponse2 = await PostCashTransactions.Endpoint(postCashTransactionsBody2, userTestObject.accessToken!);
		Assert.IsTrue(postCashTransactionsResponse2.response == "success", "Response should be success but was " + postCashTransactionsResponse2.response);

		GetTransactionsResponse transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		List<TransactionTestObject> transactionsList = TransactionHelper.CombinedTransactions(transactions);

		Assert.IsTrue(transactionsList[0].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08")), "Timestamp should be 2022-08-08 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[0].timestamp).ToString());
		Assert.IsTrue(transactionsList[0].balanceAmount == 200, "Balance should be 200 but was " + transactionsList[0].balanceAmount);
		Assert.IsTrue(transactionsList[1].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-09")), "Timestamp should be 2022-08-09 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[1].timestamp).ToString());
		Assert.IsTrue(transactionsList[1].balanceAmount == 100, "Balance should be 100 but was " + transactionsList[1].balanceAmount);
		Assert.IsTrue(transactionsList[2].balanceAmount > transactionsList[1].balanceAmount, "Blanace should be bigger than " + transactionsList[1] + " but was " + transactionsList[2].balanceAmount);
		Assert.IsTrue(transactionsList[3].balanceAmount > transactionsList[2].balanceAmount, "Blanace should be bigger than " + transactionsList[2] + " but was " + transactionsList[3].balanceAmount);
		Assert.IsTrue(transactionsList[4].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-10-20")), "Timestamp should be 2022-10-20 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[4].timestamp).ToString());
		Assert.IsTrue(transactionsList[4].balanceAmount == (transactionsList[3].balanceAmount - 100), "Balance should be " + (transactionsList[3].balanceAmount - 100) + " but was " + transactionsList[4].balanceAmount);

		PutStockTransactionsBody putStockTransactionsBody = new PutStockTransactionsBody(
			postStockTransactionsResponse.id!,
			portfolio.id!,
			15,
			Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-10-10")),
			150,
			""
		);

		PutStockTransactionsResponse putStockTransactionsResponse = await PutStockTransactions.Endpoint(userTestObject.accessToken!, putStockTransactionsBody);
		Assert.IsTrue(putStockTransactionsResponse.response == "success", "Response should be success but was " + putStockTransactionsResponse.response);

		PutCashTransactionsBody putCashTransactionsBody = new PutCashTransactionsBody(
			postCashTransactionsResponse.id!,
			portfolio.id!,
			"",
			250,
			"",
			0
		);
		PutCashTransactionsResponse putCashTransactionsResponse = await PutCashTransactions.Endpoint(putCashTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(putCashTransactionsResponse.response == "success", "Response should be success but was " + putCashTransactionsResponse.response);

		transactions = GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		transactionsList = TransactionHelper.CombinedTransactions(transactions);

		Assert.IsTrue(transactionsList[0].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-08-08")), "Timestamp should be 2022-08-08 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[0].timestamp).ToString());
		Assert.IsTrue(transactionsList[0].balanceAmount == 250, "Balance should be 200 but was " + transactionsList[0].balanceAmount);
		Assert.IsTrue(transactionsList[1].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-10-10")), "Timestamp should be 2022-10-10 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[1].timestamp).ToString());
		Assert.IsTrue(transactionsList[1].balanceAmount == 100, "Balance should be 50 but was " + transactionsList[1].balanceAmount);
		Assert.IsTrue(transactionsList[2].timestamp == Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2022-10-20")), "Timestamp should be 2022-10-20 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactionsList[2].timestamp).ToString());
		Assert.IsTrue(transactionsList[2].balanceAmount == 0, "Balance should be 0 but was " + transactionsList[2].balanceAmount);
		Assert.IsTrue(transactionsList[3].balanceAmount > transactionsList[2].balanceAmount, "Balance should be bigger than " + transactionsList[2].balanceAmount + " but was " + transactionsList[3].balanceAmount);
	}
}