namespace BackendService.tests;

using StockApp;

[TestClass]
public class TokenSetTest
{
	private UserTestObject userTestObject = null!;
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
	public void TokenSetTest_SetRefreshToken_SuccessfulTest()
	{
		TokenSet tokenSet = new TokenSet();
		tokenSet.SetRefreshToken("test");
		Assert.IsTrue(tokenSet.refreshToken == "test", "Refresh token was not set correctly. Refresh token should be \"test\" but is \"" + tokenSet.refreshToken + "\"");
	}

	[TestMethod]
	public void TokenSetTest_SetRefreshToken_RefreshNullTest()
	{
		TokenSet tokenSet = new TokenSet();
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => tokenSet.SetRefreshToken(null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void TokenSetTest_Create_SuccessfulTest()
	{
		TokenSet tokenSet = TokenSet.Create(userTestObject.user!.id!);
		Assert.IsTrue(tokenSet.accessToken != null, "Access token was not set");
		Assert.IsTrue(tokenSet.refreshToken != null, "Refresh token was not set");
	}

	[TestMethod]
	public void TokenSetTest_Create_InvalidUserIDTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => TokenSet.Create("invalid"));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void TokenSetTest_Create_NullUserIDTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => TokenSet.Create(null!));
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void TokenSetTest_Refresh_SuccessfulTest()
	{
		TokenSet tokenSet = TokenSet.Create(userTestObject.user!.id!);
		String tempRefresh = tokenSet.refreshToken!;
		String tempAccess = tokenSet.accessToken!;
		tokenSet.Refresh();
		Assert.IsTrue(tempRefresh != tokenSet.refreshToken, "Refresh token was not changed");
		Assert.IsTrue(tempAccess != tokenSet.accessToken, "Access token was not changed");
	}

	[TestMethod]
	public void TokenSetTest_Refresh_InvalidRefreshTokenTest()
	{
		TokenSet tokenSet = TokenSet.Create(userTestObject.user!.id!);
		tokenSet.refreshToken = "invalid";
		String tempAccess = tokenSet.accessToken!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => tokenSet.Refresh());
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
		Assert.IsTrue(tokenSet.refreshToken == "invalid", "Refresh token was changed from \"invalid\" to " + tokenSet.refreshToken);
		Assert.IsTrue(tempAccess == tokenSet.accessToken, "Access token was changed from " + tempAccess + " to " + tokenSet.accessToken);
	}

	[TestMethod]
	public void TokenSetTest_Refresh_NullRefreshTokenTest()
	{
		TokenSet tokenSet = TokenSet.Create(userTestObject.user!.id!);
		tokenSet.refreshToken = null!;
		String tempAccess = tokenSet.accessToken!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => tokenSet.Refresh());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
		Assert.IsTrue(tokenSet.refreshToken == null, "Refresh token was changed from null to " + tokenSet.refreshToken);
		Assert.IsTrue(tempAccess == tokenSet.accessToken, "Access token was changed from " + tempAccess + " to " + tokenSet.accessToken);
	}

	[TestMethod]
	public void TokenSetTest_GetUser_SuccessfulTest()
	{
		TokenSet tokenSet = TokenSet.Create(userTestObject.user!.id!);
		User gottenUser = tokenSet.GetUser();
		Assert.IsTrue(userTestObject.user.id == gottenUser.id, "User IDs do not match. Expected " + userTestObject.user.id + " but got " + gottenUser.id);
	}

	[TestMethod]
	public void TokenSetTest_GetUser_InvalidAccessTokenTest()
	{
		TokenSet tokenSet = TokenSet.Create(userTestObject.user!.id!);
		tokenSet.accessToken = "invalid";
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => tokenSet.GetUser());
		Assert.IsTrue(exception.StatusCode == 401, "Status code should be 401 but was " + exception.StatusCode);
	}

	[TestMethod]
	public void TokenSetTest_GetUser_NullAccessTokenTest()
	{
		TokenSet tokenSet = TokenSet.Create(userTestObject.user!.id!);
		tokenSet.accessToken = null!;
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => tokenSet.GetUser());
		Assert.IsTrue(exception.StatusCode == 400, "Status code should be 400 but was " + exception.StatusCode);
	}
}