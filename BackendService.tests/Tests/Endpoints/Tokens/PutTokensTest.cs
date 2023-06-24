namespace BackendService.tests;

[TestClass]
public class PutTokensTest
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
	public void PutTokensTest_PutTest()
	{
		TokensResponse response = PutTokens.Endpoint(userTestObject.refreshToken!);
		Assert.IsTrue(response.tokenSet.accessToken != "", "Access token should not be empty");
		Assert.IsTrue(response.tokenSet.refreshToken != "", "Refresh token should not be empty");
		Assert.IsTrue(response.tokenSet.accessToken != userTestObject.accessToken!, "The refreshed, access token should not be the same as the old one");
		Assert.IsTrue(response.tokenSet.refreshToken != userTestObject.refreshToken!, "The refreshed, refresh token should not be the same as the old one");
		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
	}

	[TestMethod]
	public void PutTokensTest_InvalidRefreshTokenTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => PutTokens.Endpoint("invalid"));
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}
}