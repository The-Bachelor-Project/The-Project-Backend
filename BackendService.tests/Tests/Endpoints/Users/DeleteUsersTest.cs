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
		DeleteUsersResponse response = DeleteUsers.Endpoint(userTestObject.user!.email!, userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => GetUsers.Endpoint(userTestObject.accessToken!));
		Assert.IsTrue(statusCodeException.StatusCode == 401, "Status code should be 401 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeleteUsersTest_InvalidUserEmailTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeleteUsers.Endpoint("invalid", userTestObject.accessToken!));
		Assert.IsTrue(statusCodeException.StatusCode == 404, "Status code should be 404 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeleteUsersTest_UnauthorizedDeletionTest()
	{
		UserTestObject userTestObject2 = UserHelper.Create();
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeleteUsers.Endpoint(userTestObject2.user!.email!, userTestObject.accessToken!));
		Assert.IsTrue(statusCodeException.StatusCode == 401, "Status code should be 401 but was " + statusCodeException.StatusCode);
		UserHelper.Delete(userTestObject2);
	}

	[TestMethod]
	public void DeleteUsersTest_NullAccessTokenTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeleteUsers.Endpoint(userTestObject.user!.email!, null!));
		Assert.IsTrue(statusCodeException.StatusCode == 400, "Status code should be 400 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeleteUsersTest_InvalidAccessTokenTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeleteUsers.Endpoint(userTestObject.user!.email!, "invalid"));
		Assert.IsTrue(statusCodeException.StatusCode == 401, "Status code should be 401 but was " + statusCodeException.StatusCode);
	}

	[TestMethod]
	public void DeleteUsersTest_NullUserEmailTest()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => DeleteUsers.Endpoint(null!, userTestObject.accessToken!));
		Assert.IsTrue(statusCodeException.StatusCode == 400, "Status code should be 400 but was " + statusCodeException.StatusCode);
	}
}