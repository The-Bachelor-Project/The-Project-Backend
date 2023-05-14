namespace BackendService.tests;

[TestClass]
public class StockTransactionTest
{
	private String ticker = "SIA";
	private String exchange = "TSE";
	private Decimal stockAmount = (decimal)120.0;
	private Decimal priceAmount = (decimal)1200.0;
	private String currency = "CAD";

	[TestMethod, Priority(1)]
	public void EndpointGetStockTransactionsFromEmptyPortfolio()
	{
		API.v1.GetStockTransactionsResponse response = API.v1.GetStockTransactions.Endpoint(Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.stockTransactions.Count == 0, "amount of stock transactions should be 0 but was " + response.stockTransactions.Count);
	}

	[TestMethod, Priority(1)]

	public async Task EndpointPostStockTransaction()
	{
		int unix = 1651055363;
		Data.StockTransaction transaction = new Data.StockTransaction(Assembly.portfolioIds[0], ticker, exchange, stockAmount, unix, new Data.Money(priceAmount, currency));
		API.v1.PostStockTransactionsBody body = new API.v1.PostStockTransactionsBody(transaction, Assembly.accessToken);
		API.v1.PostStockTransactionsResponse response = await API.v1.PostStockTransactions.EndpointAsync(body, Assembly.accessToken);
		Assembly.stockTransactionId = response.id;
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.id != null, "id should not be null");
	}

	[TestMethod, Priority(1)]
	public void EndpointGetStockTransactions()
	{
		API.v1.GetStockTransactionsResponse response = API.v1.GetStockTransactions.Endpoint(Assembly.accessToken);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assert.IsTrue(response.stockTransactions.Count == 1, "amount of stock transactions should be 1 but was " + response.stockTransactions.Count);
		Assert.IsTrue(response.stockTransactions[0].ticker == ticker, "ticker should be " + ticker + " but was " + response.stockTransactions[0].ticker);
		Assert.IsTrue(response.stockTransactions[0].exchange == exchange, "exchange should be " + exchange + " but was " + response.stockTransactions[0].exchange);
		Assert.IsTrue(response.stockTransactions[0].price.currency == currency, "currency of stock should be " + currency + " but was " + response.stockTransactions[0].price.currency);
	}

	[TestMethod, Priority(0)]
	public async Task EndpointPutStockTransactionCurrencyTest()
	{
		String newCurrency = "EUR";
		API.v1.PutStockTransactionsBody body = new API.v1.PutStockTransactionsBody(Assembly.stockTransactionId, Assembly.portfolioIds[0], 0.0m, 0, 0.0m, newCurrency);
		API.v1.PutStockTransactionsResponse response = await API.v1.PutStockTransactions.Endpoint(Assembly.accessToken, body);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response + " " + Assembly.email);
		Assert.IsTrue(response.id != Assembly.stockTransactionId, "id should not be equal to " + Assembly.stockTransactionId);
		Assembly.stockTransactionId = response.id;
		API.v1.GetStockTransactionsResponse getResponse = API.v1.GetStockTransactions.Endpoint(Assembly.accessToken);
		Assert.IsTrue(getResponse.response == "success", "response should be \"success\" but was " + getResponse.response);
		Assert.IsTrue(getResponse.stockTransactions[0].price.currency == newCurrency, "currency should be " + newCurrency + " but was " + getResponse.stockTransactions[0].price.currency);
	}


	[TestMethod, Priority(0)]
	public async Task EndpointPutStockTransactionAmountTest()
	{
		Decimal newAmount = 50.0m;
		API.v1.PutStockTransactionsBody body = new API.v1.PutStockTransactionsBody(Assembly.stockTransactionId, Assembly.portfolioIds[0], newAmount, 0, 0.0m, "");
		API.v1.PutStockTransactionsResponse response = await API.v1.PutStockTransactions.Endpoint(Assembly.accessToken, body);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response + " " + Assembly.email);
		Assert.IsTrue(response.id != Assembly.stockTransactionId, "id should not be equal to " + Assembly.stockTransactionId);
		Assembly.stockTransactionId = response.id;
		API.v1.GetStockTransactionsResponse getResponse = API.v1.GetStockTransactions.Endpoint(Assembly.accessToken);
		Assert.IsTrue(getResponse.response == "success", "response should be \"success\" but was " + getResponse.response);
		Assert.IsTrue(getResponse.stockTransactions[0].amount == newAmount, "amount should be " + newAmount + " but was " + getResponse.stockTransactions[0].amount);
	}

	[TestMethod, Priority(0)]
	public async Task EndpointPutStockTransactionTimestampTest()
	{
		int newTimestamp = 1652547;
		API.v1.PutStockTransactionsBody body = new API.v1.PutStockTransactionsBody(Assembly.stockTransactionId, Assembly.portfolioIds[0], 0.0m, newTimestamp, 0.0m, "");
		API.v1.PutStockTransactionsResponse response = await API.v1.PutStockTransactions.Endpoint(Assembly.accessToken, body);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response + " " + Assembly.email);
		Assert.IsTrue(response.id != Assembly.stockTransactionId, "id should not be equal to " + Assembly.stockTransactionId);
		Assembly.stockTransactionId = response.id;
		API.v1.GetStockTransactionsResponse getResponse = API.v1.GetStockTransactions.Endpoint(Assembly.accessToken);
		Assert.IsTrue(getResponse.response == "success", "response should be \"success\" but was " + getResponse.response);
		Assert.IsTrue(getResponse.stockTransactions[0].timestamp == newTimestamp, "amount should be " + newTimestamp + " but was " + getResponse.stockTransactions[0].timestamp);
	}

	[TestMethod, Priority(0)]
	public async Task EndpointPutStockTransactionPriceTest()
	{
		Decimal newPrice = 100.0m;
		API.v1.PutStockTransactionsBody body = new API.v1.PutStockTransactionsBody(Assembly.stockTransactionId, Assembly.portfolioIds[0], 0.0m, 0, newPrice, "");
		API.v1.PutStockTransactionsResponse response = await API.v1.PutStockTransactions.Endpoint(Assembly.accessToken, body);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response + " " + Assembly.email);
		Assert.IsTrue(response.id != Assembly.stockTransactionId, "id should not be equal to " + Assembly.stockTransactionId);
		Assembly.stockTransactionId = response.id;
		API.v1.GetStockTransactionsResponse getResponse = API.v1.GetStockTransactions.Endpoint(Assembly.accessToken);
		Assert.IsTrue(getResponse.response == "success", "response should be \"success\" but was " + getResponse.response);
		Assert.IsTrue(getResponse.stockTransactions[0].price.amount == newPrice, "amount should be " + newPrice + " but was " + getResponse.stockTransactions[0].price);
	}

	[TestMethod, Priority(0)]
	public async Task EndpointDeleteStockTransactionTest()
	{
		API.v1.DeleteStockTransactionsResponse response = await API.v1.DeleteStockTransactions.Endpoint(Assembly.accessToken, Assembly.portfolioIds[0], Assembly.stockTransactionId);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		API.v1.GetStockTransactionsResponse getResponse = API.v1.GetStockTransactions.Endpoint(Assembly.accessToken);
		Assert.IsTrue(getResponse.response == "success", "response should be \"success\" but was " + getResponse.response);
		Assert.IsTrue(getResponse.stockTransactions.Count == 0, "amount of stock transactions should be 0 but was " + getResponse.stockTransactions.Count);
	}
}

