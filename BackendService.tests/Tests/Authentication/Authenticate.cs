namespace BackendService.tests;

[TestClass]
public class AuthenticationTest
{
	[TestMethod, Priority(1)]
	public void AuthenticateInvalidTokenTest()
	{
		Boolean isValid = Authentication.Authenticate.AccessToken("NOTCORRECTTOKEN");
		Assert.IsTrue(isValid == false, "token is valid should be false but was " + isValid);
	}

	[TestMethod, Priority(1)]
	public void AuthenticateValidTokenTest()
	{
		Boolean isValid = Authentication.Authenticate.AccessToken(Assembly.accessToken);
		Assert.IsTrue(isValid == true, "token is valid should be true but was " + isValid);
	}
}