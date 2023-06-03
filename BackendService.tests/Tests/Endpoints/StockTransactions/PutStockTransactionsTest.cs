namespace BackendService.tests;

[TestClass]
public class PutStockTransactionsTest
{
	private static UserTestObject userTestObject = new UserTestObject();
	private static StockApp.StockTransaction stockTransaction = null!;

	[TestInitialize]
	public async Task Initialize()
	{
		userTestObject = UserHelper.Create();
		stockTransaction = new StockApp.StockTransaction();
		stockTransaction = await StockTransactionHelper.Create(userTestObject);
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public async Task PutStockTransactionsTest_SuccessfulPutTest()
	{
		// Put new amount
		PutStockTransactionsBody body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			((decimal)stockTransaction.amount! + 1),
			0,
			0,
			""
		);
		try
		{
			PutStockTransactionsResponse response = await PutStockTransactions.Endpoint(userTestObject.accessToken!, body);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			Assert.IsTrue(response.id != stockTransaction.id, "Stock transaction id should not be " + stockTransaction.id + " but was " + response.id);
			StockApp.StockTransaction gottenTransaction = StockTransactionHelper.Get(response.id);
			Assert.IsTrue(gottenTransaction.amount == body.newAmount, "Amount should be " + body.newAmount + " but was " + gottenTransaction.amount);
			stockTransaction = gottenTransaction;
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to put stockTransaction: " + e.Message);
		}

		// Put new price
		body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			0,
			0,
			100,
			""
		);
		try
		{
			PutStockTransactionsResponse response = await PutStockTransactions.Endpoint(userTestObject.accessToken!, body);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			Assert.IsTrue(response.id != stockTransaction.id, "Stock transaction id should not be " + stockTransaction.id + " but was " + response.id);
			StockApp.StockTransaction gottenTransaction = StockTransactionHelper.Get(response.id);
			Assert.IsTrue(gottenTransaction.priceNative!.amount == body.newPrice, "Price should be " + body.newPrice + " but was " + gottenTransaction.priceNative!.amount);
			stockTransaction = gottenTransaction;
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to put stockTransaction: " + e.Message);
		}

		// Put new currency
		body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			0,
			0,
			0,
			"EUR"
		);
		try
		{
			PutStockTransactionsResponse response = await PutStockTransactions.Endpoint(userTestObject.accessToken!, body);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			Assert.IsTrue(response.id != stockTransaction.id, "Stock transaction id should not be " + stockTransaction.id + " but was " + response.id);
			StockApp.StockTransaction gottenTransaction = StockTransactionHelper.Get(response.id);
			Assert.IsTrue(gottenTransaction.priceNative!.currency == body.newCurrency, "Currency should be " + body.newCurrency + " but was " + gottenTransaction.priceNative!.currency);
			stockTransaction = gottenTransaction;
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to put stockTransaction: " + e.Message);
		}

		// Put new currency and price
		body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			0,
			0,
			200,
			"CAD"
		);
		try
		{
			PutStockTransactionsResponse response = await PutStockTransactions.Endpoint(userTestObject.accessToken!, body);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			Assert.IsTrue(response.id != stockTransaction.id, "Stock transaction id should not be " + stockTransaction.id + " but was " + response.id);
			StockApp.StockTransaction gottenTransaction = StockTransactionHelper.Get(response.id);
			Assert.IsTrue(gottenTransaction.priceNative!.amount == body.newPrice, "Price should be " + body.newPrice + " but was " + gottenTransaction.priceNative!.amount);
			Assert.IsTrue(gottenTransaction.priceNative!.currency == body.newCurrency, "Currency should be " + body.newCurrency + " but was " + gottenTransaction.priceNative!.currency);
			stockTransaction = gottenTransaction;
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to put stockTransaction: " + e.Message);
		}

		// Put new timestamp
		body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			0,
			Tools.TimeConverter.DateTimeToUnix(DateTime.Now),
			0,
			""
		);
		try
		{
			PutStockTransactionsResponse response = await PutStockTransactions.Endpoint(userTestObject.accessToken!, body);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			Assert.IsTrue(response.id != stockTransaction.id, "Stock transaction id should not be " + stockTransaction.id + " but was " + response.id);
			StockApp.StockTransaction gottenTransaction = StockTransactionHelper.Get(response.id);
			Assert.IsTrue(gottenTransaction.timestamp == body.newTimestamp, "Timestamp should be " + body.newTimestamp + " but was " + gottenTransaction.timestamp);
			stockTransaction = gottenTransaction;
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to put stockTransaction: " + e.Message);
		}

		// Put new everything
		body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			((decimal)stockTransaction.amount! + 10),
			Tools.TimeConverter.DateTimeToUnix(DateTime.Now),
			500,
			"USD"
		);
		try
		{
			PutStockTransactionsResponse response = await PutStockTransactions.Endpoint(userTestObject.accessToken!, body);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			Assert.IsTrue(response.id != stockTransaction.id, "Stock transaction id should not be " + stockTransaction.id + " but was " + response.id);
			StockApp.StockTransaction gottenTransaction = StockTransactionHelper.Get(response.id);
			Assert.IsTrue(gottenTransaction.amount == body.newAmount, "Amount should be " + body.newAmount + " but was " + gottenTransaction.amount);
			Assert.IsTrue(gottenTransaction.timestamp == body.newTimestamp, "Timestamp should be " + body.newTimestamp + " but was " + gottenTransaction.timestamp);
			Assert.IsTrue(gottenTransaction.priceNative!.amount == body.newPrice, "Price should be " + body.newPrice + " but was " + gottenTransaction.priceNative!.amount);
			Assert.IsTrue(gottenTransaction.priceNative!.currency == body.newCurrency, "Currency should be " + body.newCurrency + " but was " + gottenTransaction.priceNative!.currency);
			stockTransaction = gottenTransaction;
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to put stockTransaction: " + e.Message);
		}
	}

	[TestMethod]
	public async Task PutStockTransactionsTest_SuccessfulPutTheSameValuesTest()
	{
		PutStockTransactionsBody body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			(decimal)stockTransaction.amount!,
			(int)stockTransaction.timestamp!,
			stockTransaction.priceNative!.amount,
			stockTransaction.priceNative!.currency
		);
		try
		{
			PutStockTransactionsResponse response = await PutStockTransactions.Endpoint(userTestObject.accessToken!, body);
			Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
			Assert.IsTrue(response.id != stockTransaction.id, "Stock transaction id should not be " + stockTransaction.id + " but was " + response.id);
			StockApp.StockTransaction gottenTransaction = StockTransactionHelper.Get(response.id);
			Assert.IsTrue(gottenTransaction.amount == body.newAmount, "Amount should be " + body.newAmount + " but was " + gottenTransaction.amount);
			Assert.IsTrue(gottenTransaction.timestamp == body.newTimestamp, "Timestamp should be " + body.newTimestamp + " but was " + gottenTransaction.timestamp);
			Assert.IsTrue(gottenTransaction.priceNative!.amount == body.newPrice, "Price should be " + body.newPrice + " but was " + gottenTransaction.priceNative!.amount);
			Assert.IsTrue(gottenTransaction.priceNative!.currency == body.newCurrency, "Currency should be " + body.newCurrency + " but was " + gottenTransaction.priceNative!.currency);
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to put stockTransaction: " + e.Message);
		}
	}

	[TestMethod]
	public void PutStockTransactionsTest_MissingStockTransactionIDTest()
	{
		PutStockTransactionsBody body = new PutStockTransactionsBody(
			0,
			stockTransaction.portfolioId!,
			(decimal)stockTransaction.amount!,
			(int)stockTransaction.timestamp!,
			stockTransaction.priceNative!.amount,
			stockTransaction.priceNative!.currency
		);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutStockTransactions.Endpoint(userTestObject.accessToken!, body).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutStockTransactionsTest_MissingPortfolioIDTest()
	{
		PutStockTransactionsBody body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			null!,
			(decimal)stockTransaction.amount!,
			(int)stockTransaction.timestamp!,
			stockTransaction.priceNative!.amount,
			stockTransaction.priceNative!.currency
		);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutStockTransactions.Endpoint(userTestObject.accessToken!, body).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutStockTransactionsTest_InvalidTest()
	{
		// Invalid currency
		PutStockTransactionsBody body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			(decimal)stockTransaction.amount!,
			(int)stockTransaction.timestamp!,
			stockTransaction.priceNative!.amount,
			"invalid"
		);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutStockTransactions.Endpoint(userTestObject.accessToken!, body).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Invalid owner
		body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId! + 1,
			(decimal)stockTransaction.amount!,
			(int)stockTransaction.timestamp!,
			stockTransaction.priceNative!.amount,
			stockTransaction.priceNative!.currency
		);
		exception = Assert.ThrowsException<StatusCodeException>(() => PutStockTransactions.Endpoint(userTestObject.accessToken!, body).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 403 but was " + exception.StatusCode);

		// Invalid price
		body = new PutStockTransactionsBody(
			(int)stockTransaction.id!,
			stockTransaction.portfolioId!,
			(decimal)stockTransaction.amount!,
			(int)stockTransaction.timestamp!,
			-1,
			stockTransaction.priceNative!.currency
		);
		exception = Assert.ThrowsException<StatusCodeException>(() => PutStockTransactions.Endpoint(userTestObject.accessToken!, body).GetAwaiter().GetResult());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}
}