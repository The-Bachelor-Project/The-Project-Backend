namespace BackendService.tests;

[TestClass]
public class GetUsersPreferencesTest
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
	public void GetUsersPreferencesTest_GetSinglePreference()
	{
		userTestObject!.user!.PostPreference("Setting 1", "Value 1");
		GetUserPreferencesResponse response = GetUserPreferences.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.preferences.Count == 1, "Response should have 1 preference but had " + response.preferences.Count);
	}

	[TestMethod]
	public void GetUsersPreferencesTest_GetMultiplePreferences()
	{
		userTestObject!.user!.PostPreference("Setting 1", "Value 1");
		userTestObject!.user!.PostPreference("Setting 2", "Value 2");
		GetUserPreferencesResponse response = GetUserPreferences.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.preferences.Count == 2, "Response should have 2 preferences but had " + response.preferences.Count);
	}

	[TestMethod]
	public void GetUsersPreferencesTest_GetMultiplePreferencesAfterChangeTest()
	{
		userTestObject!.user!.PostPreference("Setting 1", "Value 1");
		userTestObject!.user!.PostPreference("Setting 2", "Value 2");
		userTestObject!.user!.PostPreference("Setting 1", "Value 3");
		GetUserPreferencesResponse response = GetUserPreferences.Endpoint(userTestObject.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.preferences.Count == 2, "Response should have 2 preferences but had " + response.preferences.Count);
		Assert.IsTrue(response.preferences["Setting 1"] == "Value 3", "Response should have Value 3 but had " + response.preferences["Setting 1"]);
		Assert.IsTrue(response.preferences["Setting 2"] == "Value 2", "Response should have Value 2 but had " + response.preferences["Setting 2"]);
	}

	[TestMethod]
	public void GetUsersPreferencesTest_EmptyPreferencesTest()
	{
		GetUserPreferencesResponse response = GetUserPreferences.Endpoint(userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.preferences.Count == 0, "Response should have 0 preferences but had " + response.preferences.Count);
	}

	[TestMethod]
	public void GetUsersPreferencesTest_InvalidAccessTokenTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetUserPreferences.Endpoint("invalid"));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void GetUsersPreferencesTest_MissingAndNullAccessTokenTest()
	{
		// Missing
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => GetUserPreferences.Endpoint(""));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);

		// Null
		exception = Assert.ThrowsException<StatusCodeException>(() => GetUserPreferences.Endpoint(null!));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}
}