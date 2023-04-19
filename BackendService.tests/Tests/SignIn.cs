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

	// 	[TestMethod]
	// 	public void FailOnEmail()
	// 	{
	// 		SignInHelper.reset();
	// 		SignInResponse signIn = SignIn.endpoint(new SignInBody
	// 		(
	// 			SignUpHelper.getPassword() + SignUpHelper.getEmail(),
	// 			"MSTest",
	// 			SignUpHelper.getPassword()
	// 		));
	// 		Assert.IsTrue(signIn.response == "error", "signIn.response was \"" + signIn.response + "\"");
	// 		Assert.IsFalse(signIn.response == "success", "signIn.response was \"" + signIn.response + "\"");
	// 	}

	// 	[TestMethod]
	// 	public void FailOnPassword()
	// 	{
	// 		SignInHelper.reset();
	// 		SignInResponse signIn = SignIn.endpoint(new SignInBody
	// 		(
	// 			SignUpHelper.getEmail(),
	// 			"MSTest",
	// 			SignUpHelper.getPassword() + SignUpHelper.getPassword()
	// 		));
	// 		Assert.IsTrue(signIn.response == "error", "signIn.response was \"" + signIn.response + "\"");
	// 		Assert.IsFalse(signIn.response == "success", "signIn.response was \"" + signIn.response + "\"");
	// 	}
}