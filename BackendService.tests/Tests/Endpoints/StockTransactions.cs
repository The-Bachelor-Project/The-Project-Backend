// namespace BackendService.tests;

// [TestClass]
// public class StockTransactionTest
// {
// 	private String ticker = "SIA";
// 	private String exchange = "TSE";
// 	private String stockTransactionId = "";
// 	private Decimal stockAmount = (decimal)120.0;
// 	private Decimal priceAmount = (decimal)1200.0;
// 	private String currency = "CAD";
// 	[TestMethod]
// 	
// 	public async Task EndpointPostStockTransaction()
// 	{
// 		int unix = 1651055363;
// 		Data.StockTransaction transaction = new Data.StockTransaction(Assembly.portfolioIds[0], ticker, exchange, stockAmount, unix, new Data.Money(priceAmount, currency));
// 		API.v1.PostStockTransactionsBody body = new API.v1.PostStockTransactionsBody(transaction, Assembly.accessToken);
// 		API.v1.PostStockTransactionsResponse response = await API.v1.PostStockTransactions.EndpointAsync(body);
// 		stockTransactionId = response.id;
// 		// Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
// 		Assert.IsTrue(response.id != null, "id should not be null");
// 	}

// 	[TestMethod]
// 	public void EndpointGetStockTransactions()
// 	{
// 		API.v1.GetStockTransactionsResponse response = API.v1.GetStockTransactions.Endpoint(stockTransactionId, ticker, exchange, Assembly.portfolioIds[0], Assembly.accessToken);
// 		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
// 		Assert.IsTrue(response.stockTransactions.Count == 1, "amount of stock transactions should be 1 but was " + response.stockTransactions.Count);
// 		Assert.IsTrue(response.stockTransactions[0].ticker == ticker, "ticker should be " + ticker + " but was " + response.stockTransactions[0].ticker);
// 		Assert.IsTrue(response.stockTransactions[0].exchange == exchange, "exchange should be " + exchange + " but was " + response.stockTransactions[0].exchange);
// 		Assert.IsTrue(response.stockTransactions[0].amount == stockAmount, "number of stocks in transaction should be " + stockAmount + " but was " + response.stockTransactions[0].amount);
// 		Assert.IsTrue(response.stockTransactions[0].price.amount == priceAmount, "price of stock should be " + priceAmount + " but was " + response.stockTransactions[0].price.amount);
// 		Assert.IsTrue(response.stockTransactions[0].price.currency == currency, "currency of stock should be " + currency + " but was " + response.stockTransactions[0].price.currency);
// 	}

// 	[TestMethod]
// 	public void EndpointGetStockTransactionsFromEmptyPortfolio()
// 	{
// 		API.v1.GetStockTransactionsResponse response = API.v1.GetStockTransactions.Endpoint(stockTransactionId, ticker, exchange, Assembly.portfolioIds[1], Assembly.accessToken);
// 		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
// 		Assert.IsTrue(response.stockTransactions.Count == 0, "amount of stock transactions should be 0 but was " + response.stockTransactions.Count);
// 	}
// }

//TODO: Fix stock transaction returning error, even if it succeeds, as stocktransaction id is not being set
