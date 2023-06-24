namespace BackendService.tests;

[TestClass]
public class DeleteUsersPreferencesTest
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
	public void DeleteUsersPreferencesTest_SuccessfulDeletionTest()
	{
		userTestObject!.user!.PostPreference("Setting", "Value");
		DeleteUserPreferencesResponse response = DeleteUserPreferences.Endpoint("Setting", userTestObject!.accessToken!);
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Dictionary<String, String> preferences = userTestObject!.user!.GetPreferences();
		Assert.IsTrue(preferences.Count == 0, "Preferences should be empty but was " + preferences.Count);
	}

	[TestMethod]
	public void DeleteUsersPreferencesTest_MissingValuesTest()
	{
		// Setting
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteUserPreferences.Endpoint("", userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Access token
		exception = Assert.ThrowsException<StatusCodeException>(() => DeleteUserPreferences.Endpoint("Setting", ""));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

	}

	[TestMethod]
	public void DeleteUsersPreferencesTest_NullValuesTest()
	{
		// Setting
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => DeleteUserPreferences.Endpoint(null!, userTestObject!.accessToken!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);

		// Access token
		exception = Assert.ThrowsException<StatusCodeException>(() => DeleteUserPreferences.Endpoint("Setting", null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}


}