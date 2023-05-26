namespace BackendService.tests;


[TestClass]
public class AuthenticateTest
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
	public void AuthenticateTest_SuccessfulAccessToken()
	{
		String valid = Authentication.Authenticate.AccessToken(userTestObject.accessToken!);
		Assert.IsTrue(valid == "Valid", "valid should be \"Valid\" but was \"" + valid + "\"");
	}

	[TestMethod]
	public void AuthenticateTest_InvalidAccessToken()
	{
		String valid = Authentication.Authenticate.AccessToken("InvalidAccessToken");
		Assert.IsTrue(valid == "Invalid", "valid should be \"Invalid\" but was \"" + valid + "\"");
	}

	[TestMethod]
	public void AuthenticateTest_SuccessfulRefreshToken()
	{
		Authentication.RefreshAuthenticationResponse response = Authentication.Authenticate.RefreshToken(userTestObject.refreshToken!);
		Assert.IsTrue(response.error == null, "response error should be null but was \"" + response.error + "\"");
	}

	[TestMethod]
	public void AuthenticateTest_InvalidRefreshToken()
	{
		Authentication.RefreshAuthenticationResponse response = Authentication.Authenticate.RefreshToken("InvalidRefreshToken");
		Assert.IsTrue(response.error == "Invalid", "response error should be \"Invalid\" but was \"" + response.error + "\"");
	}


	// These two always need to be last in this file, because they make the tokens expired
	[TestMethod]
	public void AuthenticateTest_ExpiredAccessToken()
	{
		TokenHelper.MakeTokensExpired(userTestObject.accessToken!, userTestObject.refreshToken!);
		String valid = Authentication.Authenticate.AccessToken(userTestObject.accessToken!);
		Assert.IsTrue(valid == "Expired", "valid should be \"Expired\" but was \"" + valid + "\"");
	}

	[TestMethod]
	public void AuthenticateTest_ExpiredRefreshToken()
	{
		TokenHelper.MakeTokensExpired(userTestObject.accessToken!, userTestObject.refreshToken!);
		Authentication.RefreshAuthenticationResponse response = Authentication.Authenticate.RefreshToken(userTestObject.refreshToken!);
		Assert.IsTrue(response.error == "Expired", "response error should be \"Expired\" but was \"" + response.error + "\"");
	}
}