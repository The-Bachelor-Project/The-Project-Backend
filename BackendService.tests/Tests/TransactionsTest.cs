namespace BackendService.tests;
using System.Data.SqlClient;

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

	[ClassInitialize]
	public static async Task ClassInitialize(TestContext context)
	{
		Data.Fetcher.StockFetcher stockFetcher = new Data.Fetcher.StockFetcher();
		await stockFetcher.GetHistory("O", "NYSE", DateOnly.Parse("2022-01-01"), DateOnly.Parse("2023-01-01"), "daily", "USD");
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
		PostStockTransactionsResponse response = await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		GetTransactionsResponse transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.transactions.Where(x => x.type == "DividendPayout").Count() > 0, "Dividend payouts should be bigger than 0");

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
		PostStockTransactionsResponse response2 = await PostStockTransactions.Endpoint(postStockTransactionsBody2, userTestObject.accessToken!);
		Assert.IsTrue(response2.response == "success", "Response should be success but was " + response2.response);
		transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.transactions.Where(x => x.type == "DividendPayout").Count() > 0, "Dividend payouts should be bigger than 0");
		Assert.IsTrue(transactions.transactions.Where(x => x.type == "DividendPayout").Last().timestamp < Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")),
		"Last dividend payout should be 2023-01-01 but was " + Tools.TimeConverter.UnixTimeStampToDateOnly(transactions.transactions.Where(x => x.type == "DividendPayout").Last().timestamp));

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
		PostStockTransactionsResponse response3 = await PostStockTransactions.Endpoint(postStockTransactionsBody3, userTestObject.accessToken!);
		Assert.IsTrue(response3.response == "success", "Response should be success but was " + response3.response);
		transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
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
		PostStockTransactionsResponse response4 = await PostStockTransactions.Endpoint(postStockTransactionsBody4, userTestObject.accessToken!);
		Assert.IsTrue(response4.response == "success", "Response should be success but was " + response4.response);
		transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions.transactions.Where(x => x.type == "DividendPayout" && x.timestamp > Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-01-01")) && x.timestamp < Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2023-03-03"))).Count() == 0, "There should be no dividend payout between 2023-01-01 and 2023-03-03 but there was one");
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
		PostStockTransactionsResponse response = await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!);
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
		response = await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StockApp.StockTransaction gottenStockTransaction2 = StockTransactionHelper.Get(response.id!);
		Assert.IsTrue(gottenStockTransaction1.amountOwned == 10, "Amount owned should be 10 but was " + gottenStockTransaction1.amountOwned);
		Assert.IsTrue(gottenStockTransaction2.amountOwned == 0, "Amount owned should be 0 but was " + gottenStockTransaction2.amountOwned);
		GetTransactionsResponse transactions1 = await GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
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
		Assert.IsTrue(gottenStockTransaction1.amountOwned == 5, "Amount owned should be 5 but was " + gottenStockTransaction1.amountOwned);
		GetTransactionsResponse transactions2 = await GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions2.transactions.Where(x => x.type == "DividendPayout").Count() > transactions1.transactions.Where(x => x.type == "DividendPayout").Count(), "Dividend payouts should be more than " + transactions1.transactions.Where(x => x.type == "DividendPayout").Count() + " but was " + transactions2.transactions.Where(x => x.type == "DividendPayout").Count());

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
		gottenStockTransaction1 = StockTransactionHelper.Get(putResponse.id!);
		Assert.IsTrue(gottenStockTransaction1.amountOwned == 10, "Amount owned should be 10 but was " + gottenStockTransaction1.amountOwned);
		GetTransactionsResponse transactions3 = await GetTransactions.Endpoint(userTestObject.accessToken!, "EUR");
		Assert.IsTrue(transactions3.transactions.Where(x => x.type == "DividendPayout").Count() > transactions1.transactions.Where(x => x.type == "DividendPayout").Count() && transactions3.transactions.Where(x => x.type == "DividendPayout").Count() < transactions2.transactions.Where(x => x.type == "DividendPayout").Count(), "Dividend payouts should be more than " + transactions1.transactions.Where(x => x.type == "DividendPayout").Count() + " but was " + transactions3.transactions.Where(x => x.type == "DividendPayout").Count() + " and less than " + transactions2.transactions.Where(x => x.type == "DividendPayout").Count());
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

		PostStockTransactionsResponse postStockTransactionsResponse = await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!);
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
		PostStockTransactionsResponse postStockTransactionsResponse2 = await PostStockTransactions.Endpoint(postStockTransactionsBody2, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse2.response == "success", "Response should be success but was " + postStockTransactionsResponse2.response);

		GetTransactionsResponse transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(transactions.transactions[0].balance.amount == 200, "Balance should be 200 but was " + transactions.transactions[0].balance.amount);
		Assert.IsTrue(transactions.transactions[1].balance.amount == 100, "Balance should be 100 but was " + transactions.transactions[1].balance.amount);
		Assert.IsTrue(transactions.transactions[2].balance.amount == 50, "Balance should be 50 but was " + transactions.transactions[2].balance.amount);
		Assert.IsTrue(transactions.transactions[3].balance.amount > 50, "Balance should be 50 but was " + transactions.transactions[3].balance.amount + " " + transactions.transactions[3].description + " " + transactions.transactions[3].portfolio);
		Assert.IsTrue(transactions.transactions[4].balance.amount == (transactions.transactions[3].balance.amount + 100), "Balance should be " + (transactions.transactions[3].balance.amount + 100) + " but was " + transactions.transactions[4].balance.amount);
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
		PostStockTransactionsResponse postStockTransactionsResponse = await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!);
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
		PostStockTransactionsResponse postStockTransactionsResponse2 = await PostStockTransactions.Endpoint(postStockTransactionsBody2, userTestObject.accessToken!);

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

		GetTransactionsResponse transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(transactions.response == "success", "Response should be success but was " + transactions.response);
		Assert.IsTrue(transactions.transactions[0].balance.amount == 200, "Balance should be 200 but was " + transactions.transactions[0].balance.amount);
		Assert.IsTrue(transactions.transactions[1].balance.amount == 100, "Balance should be 100 but was " + transactions.transactions[1].balance.amount);
		Assert.IsTrue(transactions.transactions[2].balance.amount > transactions.transactions[1].balance.amount, "Balance should be greater than " + transactions.transactions[1].balance.amount + " but was " + transactions.transactions[2].balance.amount);
		Assert.IsTrue(transactions.transactions[3].balance.amount == (transactions.transactions[2].balance.amount - 50), "Balance should be " + (transactions.transactions[2].balance.amount - 50) + " but was " + transactions.transactions[3].balance.amount);
		Assert.IsTrue(transactions.transactions[4].balance.amount > transactions.transactions[3].balance.amount, "Balance should be greater than " + transactions.transactions[3].balance.amount + " but was " + transactions.transactions[4].balance.amount);
		Assert.IsTrue(transactions.transactions[5].balance.amount == (transactions.transactions[4].balance.amount - 100), "Balance should be " + (transactions.transactions[4].balance.amount - 100) + " but was " + transactions.transactions[5].balance.amount);

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

		transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(transactions.response == "success", "Response should be success but was " + transactions.response);
		Assert.IsTrue(transactions.transactions[0].balance.amount == 250, "Balance should be 250 but was " + transactions.transactions[0].balance.amount + " " + transactions.transactions[0].portfolio);
		Assert.IsTrue(transactions.transactions[1].balance.amount == 200, "Balance should be 200 but was " + transactions.transactions[1].balance.amount);
		Assert.IsTrue(transactions.transactions[2].balance.amount == 50, "Balance should be 50 but was " + transactions.transactions[2].balance.amount);
		Assert.IsTrue(transactions.transactions[3].balance.amount == -50, "Balance should be -50 but was " + transactions.transactions[3].balance.amount);
		Assert.IsTrue(transactions.transactions[4].balance.amount > transactions.transactions[3].balance.amount, "Balance should be greater than " + transactions.transactions[3].balance.amount + " but was " + transactions.transactions[4].balance.amount);
		Assert.IsTrue(transactions.transactions[4].type == "DividendPayout", "Type should be DividendPayout but was " + transactions.transactions[4].type);
	}

	[TestMethod]
	public async Task TransactionsTest_DividendPayout_AutoInsertionOfDividendPayoutsTest()
	{
		String getStartEndTrackingQuery = "SELECT start_tracking_date,end_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@ticker", "T");
		parameters.Add("@exchange", "NYSE");
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getStartEndTrackingQuery, parameters);
		String startTrackingDate = "2021-01-01";
		String endTrackingDate = "2022-01-01";
		if (data != null)
		{
			if (data["start_tracking_date"] != DBNull.Value && data["end_tracking_date"] != DBNull.Value)
			{
				startTrackingDate = Tools.TimeConverter.dateOnlyToString(DateOnly.FromDateTime((DateTime)data["start_tracking_date"]));
				endTrackingDate = Tools.TimeConverter.dateOnlyToString(DateOnly.FromDateTime((DateTime)data["end_tracking_date"]));
			}
		}
		String resetStockQuery = "EXEC ResetIt @ID, @Exchange, @IsStock";
		SqlCommand resetStockCommand = new SqlCommand(resetStockQuery, Data.Database.Connection.GetSqlConnection());
		resetStockCommand.Parameters.AddWithValue("@ID", "T");
		resetStockCommand.Parameters.AddWithValue("@Exchange", "NYSE");
		resetStockCommand.Parameters.AddWithValue("@IsStock", 1);
		try
		{
			resetStockCommand.ExecuteNonQuery();
		}
		catch (System.Exception e)
		{
			Assert.Fail("Failed to reset stock: " + e);
		}

		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.ticker = "T";
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.amount = 10;
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2021-01-02"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse = await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse.response == "success", "Response should be success but was " + postStockTransactionsResponse.response);
		GetTransactionsResponse transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(transactions.response == "success", "Response should be success but was " + transactions.response);
		Assert.IsTrue(transactions.transactions.Count == 1, "Transactions should be 1 but was " + transactions.transactions.Count);
		GetValueHistoryResponse valueHistory = await GetValueHistory.EndpointAsync(startTrackingDate, endTrackingDate, "EUR", userTestObject.accessToken!);
		Assert.IsTrue(valueHistory.valueHistory.valueHistory[0].highPrice.currency == "EUR", "Currency should be EUR but was " + valueHistory.valueHistory.valueHistory[0].highPrice.currency);
		transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, "USD");
		Assert.IsTrue(transactions.response == "success", "Response should be success but was " + transactions.response);
		Assert.IsTrue(transactions.transactions.Count >= 5, "There should be 5 or more transactions but was " + transactions.transactions.Count + " " + transactions.transactions[0].portfolio);
	}

	[TestMethod]
	public async Task TransactionsTest_SellMoreThanOwnedTest()
	{
		StockApp.StockTransaction stockTransactionData = new StockApp.StockTransaction();
		stockTransactionData.portfolioId = portfolio.id;
		stockTransactionData.ticker = "T";
		stockTransactionData.exchange = "NYSE";
		stockTransactionData.amount = 10;
		stockTransactionData.timestamp = Tools.TimeConverter.DateOnlyToUnix(DateOnly.Parse("2021-01-02"));
		stockTransactionData.priceNative = new StockApp.Money(100, "USD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		PostStockTransactionsResponse postStockTransactionsResponse = await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postStockTransactionsResponse.response == "success", "Response should be success but was " + postStockTransactionsResponse.response);
		stockTransactionData.amount = -11;
		postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransactionData.portfolioId!,
			stockTransactionData.ticker,
			stockTransactionData.exchange,
			stockTransactionData.amount,
			stockTransactionData.timestamp,
			stockTransactionData.priceNative
		);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await PostStockTransactions.Endpoint(postStockTransactionsBody, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}
}