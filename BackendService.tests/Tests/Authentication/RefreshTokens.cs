namespace BackendService.tests;

[TestClass]
public class RefreshTokensTest
{
	[TestMethod, Priority(0)]
	public void RefreshTokensInvalidTokenTest()
	{
		StockApp.TokenSet response = Authentication.RefreshTokens.All("NON_EXISTENT_TOKEN");
		Assert.IsTrue(response.refreshToken == "error", "response on refreshtoken should be \"error\" but was " + response.refreshToken);
	}

	[TestMethod, Priority(0)]
	public void RefreshTokensValidTokenTest()
	{
		StockApp.TokenSet response = Authentication.RefreshTokens.All(Assembly.refreshToken);
		Assert.IsTrue(response.refreshToken != "error", "response on refreshtoken should not be \"error\" but was " + response.refreshToken);
		Assert.IsTrue(response.refreshToken != Assembly.refreshToken, "response on refreshtoken should not be the same as the old token but was " + response.refreshToken);
		Assert.IsTrue(response.accessToken != Assembly.accessToken, "response on accesstoken should not be the same as the old token but was " + response.accessToken);
	}
}