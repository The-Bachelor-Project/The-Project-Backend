using BackendService;
using API.v1;
namespace BackendService.tests;

[TestClass]
public class SignInTest
{
	[TestMethod]
	public void Succeed()
	{
		SignInHelper.Reset();
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.GetEmail(),
			SignUpHelper.GetPassword()
		));
		System.Console.WriteLine(SignIn.response);
		Assert.IsFalse(SignIn.response == "", "signIn.response was \"" + SignIn.response + "\"");
		Assert.IsTrue(SignIn.response == "success", "signIn.response was \"" + SignIn.response + "\"");
	}

	[TestMethod]
	public void FailOnEmail()
	{
		SignInHelper.Reset();
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.GetPassword() + SignUpHelper.GetEmail(),
			SignUpHelper.GetPassword()
		));
		Assert.IsTrue(SignIn.response == "error", "signIn.response was \"" + SignIn.response + "\"");
		Assert.IsFalse(SignIn.response == "success", "signIn.response was \"" + SignIn.response + "\"");
	}

	[TestMethod]
	public void FailOnPassword()
	{
		SignInHelper.Reset();
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.GetEmail(),
			SignUpHelper.GetPassword() + SignUpHelper.GetPassword()
		));
		Assert.IsTrue(SignIn.response == "error", "signIn.response was \"" + SignIn.response + "\"");
		Assert.IsFalse(SignIn.response == "success", "signIn.response was \"" + SignIn.response + "\"");
	}
}