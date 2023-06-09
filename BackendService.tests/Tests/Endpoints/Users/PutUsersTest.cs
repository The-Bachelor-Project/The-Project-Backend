namespace BackendService.tests;

[TestClass]
public class PutUsersTest
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
	public void PutUsersTest_SuccessfulEmailChange()
	{
		String newEmail = Tools.RandomString.Generate(20) + "@test.com";
		PutEmailBody body = new PutEmailBody(newEmail);
		PutUserResponse response = PutUsers.EndpointEmail(userTestObject.accessToken!, body);
		Assert.IsTrue(response.response == "success", "response should be success but was \"" + response.response + "\"");
		userTestObject.user!.email = newEmail;
		PostTokensBody signInBody = new PostTokensBody(userTestObject.user!.email!, userTestObject.user!.password!);
		StockApp.TokenSet signInResponse = PostTokens.Endpoint(signInBody);
		Assert.IsTrue(signInResponse.accessToken != "", "accessToken should not be empty");
		Assert.IsTrue(signInResponse.refreshToken != "", "refreshToken should not be empty");
		userTestObject.accessToken = signInResponse.accessToken;
		userTestObject.refreshToken = signInResponse.refreshToken;

	}

	[TestMethod]
	public void PutUsersTest_InvalidEmail()
	{
		PutEmailBody body = new PutEmailBody(Tools.RandomString.Generate(20));
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutUsers.EndpointEmail(userTestObject.accessToken!, body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutUsersTest_SuccessfulPasswordChange()
	{
		String newPassword = Tools.RandomString.Generate(20);
		PutPasswordBody body = new PutPasswordBody(userTestObject.user!.password!, newPassword);
		PutUserResponse response = PutUsers.EndpointPass(userTestObject.accessToken!, body);
		Assert.IsTrue(response.response == "success", "response should be success but was \"" + response.response + "\"");
		userTestObject.user!.password = newPassword;
		PostTokensBody signInBody = new PostTokensBody(userTestObject.user!.email!, userTestObject.user!.password!);
		StockApp.TokenSet signInResponse = PostTokens.Endpoint(signInBody);
		Assert.IsTrue(signInResponse.accessToken != "", "accessToken should not be empty");
		Assert.IsTrue(signInResponse.refreshToken != "", "refreshToken should not be empty");
		userTestObject.accessToken = signInResponse.accessToken;
		userTestObject.refreshToken = signInResponse.refreshToken;
	}

	[TestMethod]
	public void PutUsersTest_WrongOldPassword()
	{
		PutPasswordBody body = new PutPasswordBody(Tools.RandomString.Generate(20), Tools.RandomString.Generate(20));
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutUsers.EndpointPass(userTestObject.accessToken!, body));
		Assert.IsTrue(exception.StatusCode == 401, "StatusCode should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutUsersTest_NewEmailNullTest()
	{
		PutEmailBody body = new PutEmailBody(null!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutUsers.EndpointEmail(userTestObject.accessToken!, body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutUsersTest_NewPasswordNullTest()
	{
		PutPasswordBody body = new PutPasswordBody(userTestObject.user!.password!, null!);
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutUsers.EndpointPass(userTestObject.accessToken!, body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void PutUsersTest_OldPasswordNullTest()
	{
		PutPasswordBody body = new PutPasswordBody(null!, Tools.RandomString.Generate(20));
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutUsers.EndpointPass(userTestObject.accessToken!, body));
		Assert.IsTrue(exception.StatusCode == 400, "StatusCode should be 400 but was " + exception.StatusCode);
	}
}