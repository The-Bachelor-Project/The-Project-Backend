namespace BackendService.tests;

[TestClass]
public class DeleteUsersTest
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
	public void DeleteUsersTest_SuccessfulDeletionTest()
	{
		DeleteUsersResponse response = DeleteUsers.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		PostTokensBody postTokensBody = new PostTokensBody(userTestObject.user!.email!, userTestObject.user!.password!);
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => PostTokens.Endpoint(postTokensBody));
		Assert.IsTrue(statusCodeException.StatusCode == 404, "Status code should be 404 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeleteUsersTest_NullAccessTokenTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeleteUsers.Endpoint(null!));
		Assert.IsTrue(statusCodeException.StatusCode == 400, "Status code should be 400 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeleteUsersTest_InvalidAccessTokenTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeleteUsers.Endpoint("invalid"));
		Assert.IsTrue(statusCodeException.StatusCode == 401, "Status code should be 401 but was " + statusCodeException.StatusCode);
	}
}