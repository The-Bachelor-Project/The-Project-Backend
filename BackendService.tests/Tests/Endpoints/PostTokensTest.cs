namespace BackendService.tests;

[TestClass]
public class PostTokensTest
{
	private static UserTestObject userTestObject = new UserTestObject();

	[ClassInitialize]
	public static void Initialize(TestContext context)
	{
		userTestObject = UserHelper.Create();
	}

	[ClassCleanup]
	public static void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public void PostTokensTest_SuccessfulLoginTest()
	{
		PostTokensBody body = new PostTokensBody(userTestObject.user!.email!, userTestObject.user!.password!);
		StockApp.TokenSet response = PostTokens.Endpoint(body);
		Assert.IsTrue(response.accessToken != "", "accessToken should not be empty");
		Assert.IsTrue(response.refreshToken != "", "refreshToken should not be empty");
	}

	[TestMethod]
	public void PostTokensTest_UserNotExistsTest()
	{
		String email = Tools.RandomString.Generate(10) + "@test.com";
		PostTokensBody body = new PostTokensBody(email, userTestObject.user!.password!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 404, "StatusCode should be 404 but was " + exception.StatusCode);
		Assert.IsTrue(exception.Message == "No user with the email: " + email + " was found", "Message should be \"No user with the email: " + email + " was found\" but was \"" + exception.Message + "\"");
	}

	[TestMethod]
	public void PostTokensTest_WrongPasswordTest()
	{
		String password = Tools.RandomString.Generate(10);
		PostTokensBody body = new PostTokensBody(userTestObject.user!.email!, password);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 401, "StatusCode should be 401 but was " + exception.StatusCode);
		Assert.IsTrue(exception.Message == "The password is incorrect", "Message should be \"The password is incorrect\" but was \"" + exception.Message + "\"");
	}

	[TestMethod]
	public void PostTokensTest_EmptyEmailTest()
	{
		PostTokensBody body = new PostTokensBody("", userTestObject.user!.password!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
		Assert.IsTrue(exception.Message == "Email cannot be empty", "Message should be \"Email cannot be empty\" but was \"" + exception.Message + "\"");
	}

	[TestMethod]
	public void PostTokensTest_EmptyPasswordTest()
	{
		PostTokensBody body = new PostTokensBody(userTestObject.user!.email!, "");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
		Assert.IsTrue(exception.Message == "Password cannot be empty", "Message should be \"Password cannot be empty\" but was \"" + exception.Message + "\"");
	}
}