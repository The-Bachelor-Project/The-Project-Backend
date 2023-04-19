using BackendService;
using API.v1;
namespace BackendService.tests;

[TestClass]
public class SignInTest
{
	[TestMethod]
	public void Succeed()
	{
		SignInHelper.reset();
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.getEmail(),
			SignUpHelper.getPassword()
		));
		System.Console.WriteLine(SignIn.response);
		Assert.IsFalse(SignIn.response == "", "signIn.response was \"" + SignIn.response + "\"");
		Assert.IsTrue(SignIn.response == "success", "signIn.response was \"" + SignIn.response + "\"");
	}

	[TestMethod]
	public void FailOnEmail()
	{
		SignInHelper.reset();
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.getPassword() + SignUpHelper.getEmail(),
			SignUpHelper.getPassword()
		));
		Assert.IsTrue(SignIn.response == "error", "signIn.response was \"" + SignIn.response + "\"");
		Assert.IsFalse(SignIn.response == "success", "signIn.response was \"" + SignIn.response + "\"");
	}

	[TestMethod]
	public void FailOnPassword()
	{
		SignInHelper.reset();
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.getEmail(),
			SignUpHelper.getPassword() + SignUpHelper.getPassword()
		));
		Assert.IsTrue(SignIn.response == "error", "signIn.response was \"" + SignIn.response + "\"");
		Assert.IsFalse(SignIn.response == "success", "signIn.response was \"" + SignIn.response + "\"");
	}
}