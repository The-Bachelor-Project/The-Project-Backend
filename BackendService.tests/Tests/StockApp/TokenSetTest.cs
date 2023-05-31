namespace BackendService.tests;

using StockApp;

[TestClass]
public class TokenSetTest
{
	[TestMethod]
	public void TokenSetTest_SetRefreshTokenTest()
	{
		TokenSet tokenSet = new TokenSet();
		tokenSet.SetRefreshToken("test");
		Assert.IsTrue(tokenSet.refreshToken == "test", "Refresh token was not set correctly. Refresh token should be \"test\" but is \"" + tokenSet.refreshToken + "\"");
	}

	[TestMethod]
	public void TokenSetTest_CreateTokenSet_SuccessfulTest()
	{
		UserTestObject user = UserHelper.Create();
		TokenSet tokenSet = TokenSet.Create(user.user!.id!);
		Assert.IsTrue(tokenSet.accessToken != null, "Access token was not set");
		Assert.IsTrue(tokenSet.refreshToken != null, "Refresh token was not set");
		UserHelper.Delete(user);
	}

	[TestMethod]
	public void TokenSetTest_CreateTokenSet_InvalidUserIDTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => TokenSet.Create("invalid"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void TokenSetTest_RefreshTokenSet_SuccessfulTest()
	{
		UserTestObject user = UserHelper.Create();
		TokenSet tokenSet = TokenSet.Create(user.user!.id!);
		String tempRefresh = tokenSet.refreshToken!;
		String tempAccess = tokenSet.accessToken!;
		tokenSet.Refresh();
		Assert.IsTrue(tempRefresh != tokenSet.refreshToken, "Refresh token was not changed");
		Assert.IsTrue(tempAccess != tokenSet.accessToken, "Access token was not changed");
		UserHelper.Delete(user);
	}

	[TestMethod]
	public void TokenSetTest_RefreshTokenSet_InvalidRefreshTokenTest()
	{
		UserTestObject user = UserHelper.Create();
		TokenSet tokenSet = TokenSet.Create(user.user!.id!);
		tokenSet.refreshToken = "invalid";
		String tempAccess = tokenSet.accessToken!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => tokenSet.Refresh());
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
		Assert.IsTrue(tokenSet.refreshToken == "invalid", "Refresh token was changed from \"invalid\" to " + tokenSet.refreshToken);
		Assert.IsTrue(tempAccess == tokenSet.accessToken, "Access token was changed from " + tempAccess + " to " + tokenSet.accessToken);
		UserHelper.Delete(user);
	}

	[TestMethod]
	public void TokenSetTest_GetUser_SuccessfulTest()
	{
		UserTestObject user = UserHelper.Create();
		TokenSet tokenSet = TokenSet.Create(user.user!.id!);
		User gottenUser = tokenSet.GetUser();
		Assert.IsTrue(user.user.id == gottenUser.id, "User IDs do not match. Expected " + user.user.id + " but got " + gottenUser.id);
		UserHelper.Delete(user);
	}

	[TestMethod]
	public void TokenSetTest_GetUser_InvalidAccessTokenTest()
	{
		UserTestObject user = UserHelper.Create();
		TokenSet tokenSet = TokenSet.Create(user.user!.id!);
		tokenSet.accessToken = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => tokenSet.GetUser());
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
		UserHelper.Delete(user);
	}
}