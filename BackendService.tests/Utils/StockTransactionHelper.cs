namespace BackendService.tests;
using System.Data.SqlClient;

public class StockTransactionHelper
{
	public async static Task<StockApp.StockTransaction> Create(UserTestObject userTestObject)
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction stockTransaction = new StockApp.StockTransaction();
		stockTransaction.portfolioId = portfolio.id;
		stockTransaction.amount = 1;
		stockTransaction.exchange = "NASDAQ";
		stockTransaction.ticker = "AAPL";
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		stockTransaction.price = new StockApp.Money(100, "USD");
		try
		{
			await stockTransaction.AddToDb();
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to create stockTransaction: " + e.Message);
		}
		return stockTransaction;
	}

	public static StockApp.StockTransaction Get(int id)
	{
		String getTransactionQuery = "SELECT * FROM StockTransactions WHERE id = @id";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getTransactionQuery, parameters);
		if (data == null)
		{
			throw new StatusCodeException(404, "StockTransaction with id " + id + " not found");
		}
		StockApp.StockTransaction stockTransaction = new StockApp.StockTransaction();
		stockTransaction.id = (int)data["id"];
		stockTransaction.portfolioId = (String)data["portfolio"];
		stockTransaction.amount = (decimal)data["amount"];
		stockTransaction.exchange = (String)data["exchange"];
		stockTransaction.ticker = (String)data["ticker"];
		stockTransaction.timestamp = (int)data["timestamp"];
		stockTransaction.price = new StockApp.Money((decimal)data["price_amount"], (String)data["price_currency"]);
		stockTransaction.amountOwned = (decimal)data["amount_owned"];
		return stockTransaction;
	}

}