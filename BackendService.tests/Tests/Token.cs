using BackendService;

namespace BackendService.tests;

[TestClass]
public class TokenTest
{
	[TestMethod]
    public void Different()
    {
		SignInHelper.reset();
		String refreshToken = SignInHelper.getRefreshToken();
		RefreshTokensResponse token = RefreshTokens.endpoint(new RefreshTokensBody
		(
			refreshToken
		));
		Assert.IsFalse(refreshToken == token.refreshToken, refreshToken + "==" + token.refreshToken + ", these should be different");
	}

	[TestMethod]
    public void SecondUseError()
    {
		SignInHelper.reset();
		String refreshToken = SignInHelper.getRefreshToken();
		RefreshTokensResponse token1 = RefreshTokens.endpoint(new RefreshTokensBody
		(
			refreshToken
		));
		RefreshTokensResponse token2 = RefreshTokens.endpoint(new RefreshTokensBody
		(
			refreshToken
		));
		Assert.IsTrue(token1.response == "success", "token1.response was " + token1.response);
		Assert.IsFalse(token2.response == "success", "token2.response was " + token2.response);
	}
}