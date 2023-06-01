namespace BackendService.tests;

[TestClass]
public class UserTest
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

	private void UserExistsInDatabaseCheck(String email)
	{
		String getUserQuery = "SELECT * FROM Accounts WHERE email = @email";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@email", email);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getUserQuery, parameters);
		Assert.IsTrue(data != null, "Account is not in database");
	}

	[TestMethod]
	public void UserTest_SingUp_SuccessfulTest()
	{
		String email = Tools.RandomString.Generate(200) + "@test.com";
		String password = Tools.RandomString.Generate(10);
		StockApp.User user = new StockApp.User(email, password);
		try
		{
			user.SignUp();
			UserExistsInDatabaseCheck(email);
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, user not added to database: " + e.Message);
		}

		UserHelper.Delete(new UserTestObject(user, "", "", -2));
	}

	[TestMethod]
	public void UserTest_SignUp_UserAlreadyExistsTest()
	{
		String email = Tools.RandomString.Generate(200) + "@test.com";
		String password = Tools.RandomString.Generate(10);
		StockApp.User user = new StockApp.User(email, password);
		try
		{
			user.SignUp();
			UserExistsInDatabaseCheck(email);
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, user not added to database: " + e.Message);
		}
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.SignUp());
		Assert.IsTrue(exception.StatusCode == 409, "status code should be 409 but was " + exception.StatusCode);
		UserHelper.Delete(new UserTestObject(user, "", "", -2));
	}

	[TestMethod]
	public void UserTest_SignUp_EmailNullTest()
	{
		String password = Tools.RandomString.Generate(10);
		StockApp.User user = new StockApp.User(null!, password);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.SignUp());
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_SignUp_PasswordNullTest()
	{
		String email = Tools.RandomString.Generate(200) + "@test.com";
		StockApp.User user = new StockApp.User(email, null!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.SignUp());
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_SignIn_SuccessfulTest()
	{
		StockApp.User user = userTestObject.user!;
		try
		{
			user.SignIn();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, user not added to database: " + e.Message);
		}
	}

	[TestMethod]
	public void UserTest_SignIn_UserDoesNotExistTest()
	{
		StockApp.User user = userTestObject.user!;
		String tempEmailHolder = user.email!;
		user.email = Tools.RandomString.Generate(200) + "@test.com";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.SignIn());
		Assert.IsTrue(exception.StatusCode == 404, "status code should be 401 but was " + exception.StatusCode);
		user.email = tempEmailHolder;
	}

	[TestMethod]
	public void UserTest_SignIn_WrongPasswordTest()
	{
		StockApp.User user = userTestObject.user!;
		user.password = Tools.RandomString.Generate(10);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.SignIn());
		Assert.IsTrue(exception.StatusCode == 401, "status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_SignIn_EmailNullTest()
	{
		StockApp.User user = new StockApp.User(null!, Tools.RandomString.Generate(10));
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.SignIn());
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_SignIn_PasswordNullTest()
	{
		StockApp.User user = new StockApp.User(Tools.RandomString.Generate(200) + "@test.com", null!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.SignIn());
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangeEmail_SuccessfulTest()
	{
		StockApp.User user = userTestObject.user!;
		String newEmail = Tools.RandomString.Generate(200) + "@test.com";
		try
		{
			user.ChangeEmail(user.email!, newEmail);
			UserExistsInDatabaseCheck(newEmail);
			user.email = newEmail;
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, email was not changed: " + e.Message);
		}
	}

	[TestMethod]
	public void UserTest_ChangeEmail_EmailAlreadyExistsTest()
	{
		StockApp.User user = userTestObject.user!;
		String otherEmail = Tools.RandomString.Generate(200) + "@test.com";
		StockApp.User otherUser = new StockApp.User(otherEmail, Tools.RandomString.Generate(10));
		try
		{
			otherUser.SignUp();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, user not added to database: " + e.Message);
		}
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangeEmail(user.email!, otherEmail));
		Assert.IsTrue(exception.StatusCode == 409, "status code should be 409 but was " + exception.StatusCode);
		UserHelper.Delete(new UserTestObject(otherUser, "", "", -2));
	}

	[TestMethod]
	public void UserTest_ChangeEmail_NewEmailNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangeEmail(user.email!, null!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangeEmail_OldEmailNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangeEmail(null!, Tools.RandomString.Generate(200) + "@test.com"));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangePassword_SuccessfulTest()
	{
		StockApp.User user = userTestObject.user!;
		String newPassword = Tools.RandomString.Generate(10);
		try
		{
			user.ChangePassword(user.password!, newPassword, user.email!);
			user.password = newPassword;
			user.SignIn();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, password was not changed: " + e.Message);
		}
	}

	[TestMethod]
	public void UserTest_ChangePassword_WrongPasswordTest()
	{
		StockApp.User user = userTestObject.user!;
		String newPassword = Tools.RandomString.Generate(10);
		String randomPassword = Tools.RandomString.Generate(10);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangePassword(randomPassword, newPassword, user.email!));
		Assert.IsTrue(exception.StatusCode == 401, "status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangePassword_NewPasswordNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangePassword(user.password!, null!, user.email!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangePassword_OldPasswordNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangePassword(null!, Tools.RandomString.Generate(10), user.email!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_SuccessfulDeleteTest()
	{

	}

	[TestMethod]
	public void UserTest_Delete_UserDoesNotExistTest()
	{

	}

	[TestMethod]
	public void UserTest_UpdatePortfolios_SinglePortfolioTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.User user = userTestObject.user!;
		Assert.IsTrue(user.portfolios.Count == 0, "portfolio list should be empty for a start but was " + user.portfolios.Count);
		try
		{
			user.UpdatePortfolios();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, portfolios not gotten: " + e.Message);
		}
		Assert.IsTrue(user.portfolios.Count == 1, "portfolio list should have one portfolio but has " + user.portfolios.Count);
		PortfolioHelper.Delete(userTestObject);
	}

	[TestMethod]
	public void UserTest_UpdatePortfolios_MultiplePortfoliosTest()
	{
		StockApp.Portfolio portfolio1 = PortfolioHelper.Create(userTestObject);
		StockApp.Portfolio portfolio2 = PortfolioHelper.Create(userTestObject);
		StockApp.Portfolio portfolio3 = PortfolioHelper.Create(userTestObject);
		StockApp.User user = userTestObject.user!;
		Assert.IsTrue(user.portfolios.Count == 0, "portfolio list should be empty for a start but was " + user.portfolios.Count);
		try
		{
			user.UpdatePortfolios();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, portfolios not gotten: " + e.Message);
		}
		Assert.IsTrue(user.portfolios.Count == 3, "portfolio list should have three portfolios but has " + user.portfolios.Count);
		PortfolioHelper.Delete(userTestObject);
	}

	[TestMethod]
	public void UserTest_UpdatePortfolios_EmptyPortfolioListTest()
	{
		StockApp.User user = userTestObject.user!;
		Assert.IsTrue(user.portfolios.Count == 0, "portfolio list should be empty for a start but was " + user.portfolios.Count);
		try
		{
			user.UpdatePortfolios();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, portfolios not gotten: " + e.Message);
		}
		Assert.IsTrue(user.portfolios.Count == 0, "portfolio list should be empty after updating but has " + user.portfolios.Count);
	}

	[TestMethod]
	public void UserTest_UpdatePortfolios_UserIDNullTest()
	{
		StockApp.User user = userTestObject.user!;
		user.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.UpdatePortfolios());
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_GetPortfolio_SuccessfulTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.User user = userTestObject.user!;
		try
		{
			user.UpdatePortfolios();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, portfolios not gotten: " + e.Message);
		}
		StockApp.Portfolio gottenPortfolio = user.GetPortfolio(portfolio.id!);
		Assert.IsTrue(gottenPortfolio.id == portfolio.id, "portfolio id should be " + portfolio.id + " but was " + gottenPortfolio.id);
		PortfolioHelper.Delete(userTestObject);
	}

	[TestMethod]
	public void UserTest_GetPortfolio_PortfolioIDNullTest()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.User user = userTestObject.user!;
		try
		{
			user.UpdatePortfolios();
		}
		catch (Exception e)
		{
			Assert.Fail("Exception thrown, portfolios not gotten: " + e.Message);
		}
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.GetPortfolio(null!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_Delete_SuccessfulTest()
	{
		StockApp.User user = userTestObject.user!;
		user.Delete(user.email!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetUsers.Endpoint(userTestObject.accessToken!));
		Assert.IsTrue(exception.StatusCode == 401, "status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_Delete_InvalidEmailTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.Delete(Tools.RandomString.Generate(10)));
		Assert.IsTrue(exception.StatusCode == 404, "status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_Delete_EmailNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.Delete(null!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_Delete_InvalidUserIDTest()
	{
		StockApp.User user = userTestObject.user!;
		user.id = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.Delete(user.email!));
		Assert.IsTrue(exception.StatusCode == 401, "status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_Delete_UserIDNullTest()
	{
		StockApp.User user = userTestObject.user!;
		user.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.Delete(user.email!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}
}