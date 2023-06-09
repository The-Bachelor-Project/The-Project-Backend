namespace BackendService.tests;
using System.Data.SqlClient;

[TestClass]
public class PostPortfoliosTest
{
	private static UserTestObject userTestObject = new UserTestObject();
	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}

	private void DeletePortfolioHelper(String id)
	{
		String deleteQuery = "DELETE FROM Portfolios WHERE uid = @id";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(deleteQuery, connection);
		command.Parameters.AddWithValue("@id", id);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			Assert.Fail("Portfolio could not be deleted: " + e.Message);
			System.Console.WriteLine(e);
		}
	}

	[TestMethod]
	public void PostPortfoliosTest_SuccessfulCreateTest()
	{
		PortfolioBody portfolio = new PortfolioBody("Test Portfolio", "USD");
		PostPortfoliosBody body = new PostPortfoliosBody(portfolio);
		PostPortfoliosResponse response = PostPortfolios.Endpoint(body, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != "", "Id should not be empty");

		// Delete portfolio
		DeletePortfolioHelper(response.id);
	}

	[TestMethod]
	public void PostPortfoliosTest_InvalidUserTest()
	{
		PortfolioBody portfolio = new PortfolioBody("Test Portfolio", "USD");
		PostPortfoliosBody body = new PostPortfoliosBody(portfolio);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostPortfolios.Endpoint(body, "invalid token"));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostPortfoliosTest_MissingCurrencyTest()
	{
		PortfolioBody portfolio = new PortfolioBody("Test Portfolio", "");
		PostPortfoliosBody body = new PostPortfoliosBody(portfolio);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostPortfolios.Endpoint(body, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostPortfoliosTest_MissingNameTest()
	{
		PortfolioBody portfolio = new PortfolioBody(null!, "USD");
		PostPortfoliosBody body = new PostPortfoliosBody(portfolio);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostPortfolios.Endpoint(body, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostPortfoliosTest_InvalidCurrency()
	{
		PortfolioBody portfolio = new PortfolioBody("Test Portfolio", "invalid currency");
		PostPortfoliosBody body = new PostPortfoliosBody(portfolio);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostPortfolios.Endpoint(body, userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostPortfoliosTest_CreatingWithSameID()
	{
		PortfolioBody firstPortfolio = new PortfolioBody("Test Portfolio", "USD");
		PostPortfoliosBody firstBody = new PostPortfoliosBody(firstPortfolio);
		PostPortfoliosResponse response = PostPortfolios.Endpoint(firstBody, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id != "", "Id should not be empty");

		StockApp.Portfolio portfolio = new StockApp.Portfolio(response.id, "", "", "EUR");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => portfolio.AddToDb());
		Assert.IsTrue(exception.StatusCode == 500, "Status code should be 500 but was " + exception.StatusCode);


		// Delete portfolio
		DeletePortfolioHelper(response.id);
	}

}