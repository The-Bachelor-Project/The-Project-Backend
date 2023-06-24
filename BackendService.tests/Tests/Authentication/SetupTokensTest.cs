namespace BackendService.tests;

[TestClass]
public class SetupTokensTest
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
	public void SetupTokensTest_SuccessfulSetupTest()
	{
		StockApp.TokenSet tokenSet = Authentication.SetupTokens.Call((int)userTestObject.familyID!);
		Assert.IsTrue(tokenSet.accessToken != "", "accessToken should not be empty");
		Assert.IsTrue(tokenSet.refreshToken != "", "refreshToken should not be empty");
	}

	[TestMethod]
	public void SetupTokensTest_FamilyNotExistsTest()
	{
		StatusCodeException exception = Assert.ThrowsException<StatusCodeException>(() => Authentication.SetupTokens.Call(-1));
		Assert.IsTrue(exception.StatusCode == 500, "StatusCode should be 500 but was " + exception.StatusCode);
	}
}