namespace BackendService.tests;

[TestClass]
public class PostTokensTest
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
	}

	[TestMethod]
	public void PostTokensTest_WrongPasswordTest()
	{
		String password = Tools.RandomString.Generate(10);
		PostTokensBody body = new PostTokensBody(userTestObject.user!.email!, password);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 401, "StatusCode should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostTokensTest_EmptyEmailTest()
	{
		PostTokensBody body = new PostTokensBody("", userTestObject.user!.password!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostTokensTest_EmptyPasswordTest()
	{
		PostTokensBody body = new PostTokensBody(userTestObject.user!.email!, "");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}
}