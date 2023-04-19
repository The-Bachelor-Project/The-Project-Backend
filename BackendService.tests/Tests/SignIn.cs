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
		System.Console.WriteLine(SignIn.Response);
		Assert.IsFalse(SignIn.Response == "", "signIn.response was \"" + SignIn.Response + "\"");
		Assert.IsTrue(SignIn.Response == "success", "signIn.response was \"" + SignIn.Response + "\"");
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
		Assert.IsTrue(SignIn.Response == "error", "signIn.response was \"" + SignIn.Response + "\"");
		Assert.IsFalse(SignIn.Response == "success", "signIn.response was \"" + SignIn.Response + "\"");
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
		Assert.IsTrue(SignIn.Response == "error", "signIn.response was \"" + SignIn.Response + "\"");
		Assert.IsFalse(SignIn.Response == "success", "signIn.response was \"" + SignIn.Response + "\"");
	}
}