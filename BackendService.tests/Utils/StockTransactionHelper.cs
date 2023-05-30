namespace BackendService.tests;

public class StockTransactionHelper
{
	public async static Task<StockApp.StockTransaction> CreateStockTransactionHelper(UserTestObject userTestObject)
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction stockTransaction = new StockApp.StockTransaction();
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
}