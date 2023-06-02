namespace BackendService.tests;

[TestClass]
public class PostUserPreferencesTest
{

	private UserTestObject? userTestObject;

	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject!);
	}

	[TestMethod]
	public void PostUserPreferencesTest_SuccessfulTest()
	{
		PostUserPreferencesBody body = new PostUserPreferencesBody("Test Setting", "Test Value");
		PostUserPreferencesResponse response = PostUserPreferences.Endpoint(body, userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.id > 0, "Response id should be greater than 0 but was " + response.id);

	}

	[TestMethod]
	public void PostUserPreferencesTest_MissingValues()
	{
		PostUserPreferencesBody body = new PostUserPreferencesBody("", "Test Value");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostUserPreferences.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		body = new PostUserPreferencesBody("Test Setting", "");
		exception = Assert.ThrowsException<StatusCodeException>(() => PostUserPreferences.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PostUserPreferencesTest_NullValues()
	{
		PostUserPreferencesBody body = new PostUserPreferencesBody(null!, "Test Value");
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PostUserPreferences.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		body = new PostUserPreferencesBody("Test Setting", null!);
		exception = Assert.ThrowsException<StatusCodeException>(() => PostUserPreferences.Endpoint(body, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}
}