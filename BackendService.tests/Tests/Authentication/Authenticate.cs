namespace BackendService.tests;

[TestClass]
public class AuthenticationTest
{
	[TestMethod, Priority(1)]
	public void AuthenticateInvalidTokenTest()
	{
		String isValid = Authentication.Authenticate.AccessToken("NOTCORRECTTOKEN");
		Assert.IsTrue(isValid == "Invalid", "token should be invalid but was " + isValid);
	}

	[TestMethod, Priority(1)]
	public void AuthenticateValidTokenTest()
	{
		String isValid = Authentication.Authenticate.AccessToken(Assembly.accessToken);
		Assert.IsTrue(isValid == "Valid", "token should be valid but was " + isValid);
	}
}