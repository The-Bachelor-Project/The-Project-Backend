namespace BackendService.tests;

[TestClass]
public class PostUsersTest
{
	private static String email = Tools.RandomString.Generate(10) + "@test.com";
	private static String password = Tools.RandomString.Generate(10);

	[TestCleanup]
	public void Cleanup()
	{
		StockApp.User user = new StockApp.User(email, password);
		UserTestObject userTestObject = new UserTestObject(user, "", "", -1);
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public void PostUserTest_SuccessfulSignUpTest()
	{
		PostUsersBody body = new PostUsersBody(email, password);
		PostUsersResponse response = PostUsers.Endpoint(body);
		Assert.IsTrue(response.response == "success", "response should be success but was \"" + response.response + "\"");
		Assert.IsTrue(response.uid != "", "userID should not be empty");
	}

	[TestMethod]
	public void PostUserTest_UserAlreadyExistsTest()
	{
		PostUsersBody body = new PostUsersBody(email, password);
		PostUsers.Endpoint(body);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostUsers.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 409, "StatusCode should be 409 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostUserTest_InvalidEmail()
	{
		PostUsersBody body = new PostUsersBody(Tools.RandomString.Generate(20), password);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostUsers.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}
}