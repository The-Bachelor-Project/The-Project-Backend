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
			user.ChangeEmail(newEmail, "HEL");
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
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangeEmail(otherEmail, "HEL"));
		Assert.IsTrue(exception.StatusCode == 409, "status code should be 409 but was " + exception.StatusCode);
		UserHelper.Delete(new UserTestObject(otherUser, "", "", -2));
	}

	[TestMethod]
	public void UserTest_ChangeEmail_NewEmailNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangeEmail(null!, "HEL"));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangePassword_SuccessfulTest()
	{
		StockApp.User user = userTestObject.user!;
		String newPassword = Tools.RandomString.Generate(10);
		try
		{
			user.ChangePassword(user.password!, newPassword);
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
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangePassword(randomPassword, newPassword));
		Assert.IsTrue(exception.StatusCode == 401, "status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangePassword_NewPasswordNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangePassword(user.password!, null!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_ChangePassword_OldPasswordNullTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.ChangePassword(null!, Tools.RandomString.Generate(10)));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
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
		user.UpdatePortfolios();
		Assert.IsTrue(user.portfolios.Count == 0, "portfolio list should be empty but has " + user.portfolios.Count);
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
		StockApp.Portfolio gottenPortfolio = user.GetPortfolios(portfolio.id!);
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
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.GetPortfolios(null!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_Delete_SuccessfulTest()
	{
		StockApp.User user = userTestObject.user!;
		user.Delete();
		PostTokensBody postTokensBody = new PostTokensBody(userTestObject.user!.email!, userTestObject.user!.password!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(postTokensBody));
		Assert.IsTrue(exception.StatusCode == 404, "status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_Delete_UserIDNullTest()
	{
		StockApp.User user = userTestObject.user!;
		user.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.Delete());
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_PostPreference_SuccessfulTest()
	{
		StockApp.User user = userTestObject.user!;
		user.PostPreference("test", "test");
		userTestObject = UserHelper.GetPreferences(userTestObject);
		Assert.IsTrue(userTestObject.setting == "test", "setting should be test but was " + userTestObject.setting);
		Assert.IsTrue(userTestObject.settingValue == "test", "value should be test but was " + userTestObject.settingValue);
	}

	[TestMethod]
	public void UserTest_PostPreference_NullValuesTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.PostPreference(null!, ""));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);

		exception = Assert.ThrowsException<StatusCodeException>(() => user.PostPreference("", null!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_PostPreference_UpdateExistingPreferenceTest()
	{
		StockApp.User user = userTestObject.user!;
		user.PostPreference("test", "test");
		userTestObject = UserHelper.GetPreferences(userTestObject);
		Assert.IsTrue(userTestObject.setting == "test", "setting should be test but was " + userTestObject.setting);
		Assert.IsTrue(userTestObject.settingValue == "test", "value should be test2 but was " + userTestObject.settingValue);
		user.PostPreference("test", "test2");
		userTestObject = UserHelper.GetPreferences(userTestObject);
		Assert.IsTrue(userTestObject.setting == "test", "setting should be test but was " + userTestObject.setting);
		Assert.IsTrue(userTestObject.settingValue == "test2", "value should be test2 but was " + userTestObject.settingValue);
	}

	[TestMethod]
	public void UserTest_GetPreferences_SinglePreferenceTest()
	{
		userTestObject.user!.PostPreference("setting", "value");
		userTestObject = UserHelper.GetPreferences(userTestObject);
		Assert.IsTrue(userTestObject.setting == "setting", "setting should be setting but was " + userTestObject.setting);
		Assert.IsTrue(userTestObject.settingValue == "value", "value should be value but was " + userTestObject.settingValue);
	}

	[TestMethod]
	public void UserTest_GetPreferences_MultiplePreferencesTest()
	{
		userTestObject.user!.PostPreference("setting", "value");
		userTestObject.user!.PostPreference("setting2", "value2");
		Dictionary<string, string> preferences = userTestObject.user!.GetPreferences();
		Assert.IsTrue(preferences.Count == 2, "preferences should have 2 entries but was " + preferences.Count);
		Assert.IsTrue(preferences["setting"] == "value", "setting should be value but was " + preferences["setting"]);
		Assert.IsTrue(preferences["setting2"] == "value2", "setting2 should be value2 but was " + preferences["setting2"]);
	}

	[TestMethod]
	public void UserTest_GetPreferences_EmptyPreferencesTest()
	{
		Dictionary<string, string> preferences = userTestObject.user!.GetPreferences();
		Assert.IsTrue(preferences.Count == 0, "preferences should be empty but was " + preferences.Count);
	}

	[TestMethod]
	public void UserTest_GetPreferences_NullIDTest()
	{
		StockApp.User user = userTestObject.user!;
		user.id = null;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.GetPreferences());
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_DeletePreference_SuccessfulDeletionTest()
	{
		StockApp.User user = userTestObject.user!;
		user.PostPreference("setting", "value");
		userTestObject = UserHelper.GetPreferences(userTestObject);
		Assert.IsTrue(userTestObject.setting == "setting", "setting should be setting but was " + userTestObject.setting);
		Assert.IsTrue(userTestObject.settingValue == "value", "value should be value but was " + userTestObject.settingValue);
		user.DeletePreference("setting");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => userTestObject = UserHelper.GetPreferences(userTestObject));
		Assert.IsTrue(exception.StatusCode == 500, "status code should be 500 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void UserTest_DeletePreference_NullValuesTest()
	{
		StockApp.User user = userTestObject.user!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => user.DeletePreference(null!));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);

		user.id = null!;
		exception = Assert.ThrowsException<StatusCodeException>(() => user.DeletePreference("Setting"));
		Assert.IsTrue(exception.StatusCode == 400, "status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task UserTest_GetTransactions_GetAllCurrencies()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction stockTransaction = new StockApp.StockTransaction();
		stockTransaction.portfolioId = portfolio!.id;
		stockTransaction.ticker = "VICI";
		stockTransaction.exchange = "NYSE";
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now) - 8000000;
		stockTransaction.priceNative = new StockApp.Money(100, "CAD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransaction.portfolioId!,
			stockTransaction.ticker,
			stockTransaction.exchange,
			stockTransaction.amount,
			stockTransaction.timestamp,
			stockTransaction.priceNative!
		);

		PostCashTransactionsBody postCashTransactionsBody = new PostCashTransactionsBody(
			stockTransaction.portfolioId!,
			"DKK",
			stockTransaction.priceNative.amount,
			stockTransaction.timestamp,
			"Test"
		);
		PostCashTransactionsResponse postCashTransactionsResponse = await PostCashTransactions.Endpoint(postCashTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postCashTransactionsResponse.response == "success", "response should be success but was " + postCashTransactionsResponse.response);

		foreach (String currency in Dictionaries.currencies)
		{
			GetTransactionsResponse transactions = await GetTransactions.Endpoint(userTestObject.accessToken!, currency);
			Assert.IsTrue(transactions.response == "success", "response should be success but was " + transactions.response);
			Assert.IsTrue(transactions.transactions.All(transaction => transaction.balance!.currency == currency), "currency should be " + currency + " but was " + transactions.transactions[0].balance!.currency);
			Assert.IsTrue(transactions.transactions.All(transaction => transaction.combinedBalance!.currency == currency), "currency should be " + currency + " but was " + transactions.transactions[0].combinedBalance!.currency);
			Assert.IsTrue(transactions.transactions.All(transaction => transaction.value!.currency == currency), "currency should be " + currency + " but was " + transactions.transactions[0].value!.currency);
		}
	}

	[TestMethod]
	public async Task UserTest_GetTransactions_InvalidCurrency()
	{
		StockApp.Portfolio portfolio = PortfolioHelper.Create(userTestObject);
		StockApp.StockTransaction stockTransaction = new StockApp.StockTransaction();
		stockTransaction.portfolioId = portfolio!.id;
		stockTransaction.ticker = "VICI";
		stockTransaction.exchange = "NYSE";
		stockTransaction.amount = 10;
		stockTransaction.timestamp = Tools.TimeConverter.DateTimeToUnix(DateTime.Now) - 8000000;
		stockTransaction.priceNative = new StockApp.Money(100, "CAD");
		PostStockTransactionsBody postStockTransactionsBody = new PostStockTransactionsBody(
			stockTransaction.portfolioId!,
			stockTransaction.ticker,
			stockTransaction.exchange,
			stockTransaction.amount,
			stockTransaction.timestamp,
			stockTransaction.priceNative!
		);

		PostCashTransactionsBody postCashTransactionsBody = new PostCashTransactionsBody(
			stockTransaction.portfolioId!,
			"DKK",
			stockTransaction.priceNative.amount,
			stockTransaction.timestamp,
			"Test"
		);
		PostCashTransactionsResponse postCashTransactionsResponse = await PostCashTransactions.Endpoint(postCashTransactionsBody, userTestObject.accessToken!);
		Assert.IsTrue(postCashTransactionsResponse.response == "success", "response should be success but was " + postCashTransactionsResponse.response);
		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(async () => await GetTransactions.Endpoint(userTestObject.accessToken!, "invalid"));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}
}