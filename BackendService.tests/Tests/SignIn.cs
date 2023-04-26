using BackendService;
using API.v1;
namespace BackendService.tests;

[TestClass]
public class SignInTest
{
	[TestMethod]
	public void Succeed()
	{
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			Assembly.email,
			Assembly.password
		));
		Assert.IsFalse(SignIn.response == "", "signIn.response was \"" + SignIn.response + "\"");
		Assert.IsTrue(SignIn.response == "success", "signIn.response was \"" + SignIn.response + "\"");
	}

	[TestMethod]
	public void FailOnEmail()
	{
		SignInHelper.Reset();
		Assert.ThrowsException<UserDoesNotExistException>(() =>
		{
			TokensResponse signIn = PostTokens.Endpoint(new PostTokensBody
		(
			Assembly.password + Assembly.email,
			Assembly.password
		));
		}, "User was logged in with email, even though it is wrong");
	}

	[TestMethod]
	public void FailOnPassword()
	{
		SignInHelper.Reset();
		Assert.ThrowsException<WrongPasswordException>(() =>
		{
			TokensResponse signIn = PostTokens.Endpoint(new PostTokensBody
		(
			Assembly.email,
			Assembly.password + Assembly.password
		));
		}, "User was logged in with password, even though it is wrong");
	}
}