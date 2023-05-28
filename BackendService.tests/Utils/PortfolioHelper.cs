namespace BackendService.tests;
using System.Data.SqlClient;

public class PortfolioHelper
{
	public static StockApp.Portfolio CreatePortfolioHelper(UserTestObject userTestObject)
	{
		StockApp.Portfolio portfolio = new StockApp.Portfolio("TEST", userTestObject.user!.id!, "EUR", 0, false);
		try
		{
			portfolio.AddToDb();
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to create portfolio: " + e.Message);
		}
		return portfolio;
	}

	public static void DeletePortfolioHelper(StockApp.Portfolio portfolio, UserTestObject userTestObject)
	{
		String deletePortfolioQuery = "DELETE FROM Portfolios WHERE uid = @uid";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(deletePortfolioQuery, connection);
		command.Parameters.AddWithValue("@uid", userTestObject.user!.id!);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to delete portfolio: " + e.Message);
		}
	}

	public static StockApp.Portfolio GetPortfolio(String id)
	{
		String getPortfolioQuery = "SELECT * FROM Portfolios WHERE uid = @uid";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@uid", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getPortfolioQuery, parameters);
		if (data == null)
		{
			Assert.Fail("Failed to get portfolio");
		}
		StockApp.Portfolio portfolio = new StockApp.Portfolio(data["uid"].ToString()!, data["name"].ToString()!, data["owner"].ToString()!, data["currency"].ToString()!, Convert.ToDecimal(data["balance"].ToString()!), true); // FIXME: This is a hack, add track balance to db
		return portfolio;

	}
}