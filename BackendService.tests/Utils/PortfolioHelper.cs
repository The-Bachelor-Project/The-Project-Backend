namespace BackendService.tests;
using System.Data.SqlClient;

public class PortfolioHelper
{
	public static StockApp.Portfolio Create(UserTestObject userTestObject)
	{
		StockApp.Portfolio portfolio = new StockApp.Portfolio("This one", userTestObject.user!.id!, "EUR");
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

	public static void Delete(UserTestObject userTestObject)
	{
		String deletePortfolioQuery = "DELETE FROM Portfolios WHERE owner = @owner";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(deletePortfolioQuery, connection);
		command.Parameters.AddWithValue("@owner", userTestObject.user!.id!);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			Assert.Fail("Failed to delete portfolio: " + e.Message);
		}
	}

	public static StockApp.Portfolio Get(String id)
	{
		String getPortfolioQuery = "SELECT * FROM Portfolios WHERE uid = @uid";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@uid", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getPortfolioQuery, parameters);
		if (data == null)
		{
			Assert.Fail("Failed to get portfolio");
		}
		StockApp.Portfolio portfolio = new StockApp.Portfolio(data["uid"].ToString()!, data["name"].ToString()!, data["owner"].ToString()!, data["currency"].ToString()!);
		return portfolio;

	}
}