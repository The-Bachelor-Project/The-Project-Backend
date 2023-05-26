namespace BackendService.tests;

[TestClass]
public class PostUsersTest
{
	private static String email = Tools.RandomString.Generate(10) + "@test.com";
	private static String password = Tools.RandomString.Generate(10);

	[ClassCleanup]
	public static void Cleanup()
	{
		StockApp.User user = new StockApp.User(email, password);
		UserHelper.Delete(user);
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
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostUsers.Endpoint(body));
		Assert.IsTrue(exception.StatusCode == 409, "StatusCode should be 409 but was " + exception.StatusCode);
	}
}