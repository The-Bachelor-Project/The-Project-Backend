using BackendService;

namespace BackendService.tests;

[TestClass]
public class SignInTest
{
	[TestMethod]
    public void Succeed()
    {
		SignInHelper.reset();
		SignInResponse signIn = SignIn.endpoint(new SignInBody
		(
			SignUpHelper.getEmail(),
			"MSTest",
			SignUpHelper.getPassword()
		));
		Assert.IsFalse(signIn.response == "error","signIn.response was \"" + signIn.response + "\"");
		Assert.IsTrue(signIn.response == "success","signIn.response was \"" + signIn.response + "\"");
	}

	[TestMethod]
    public void FailOnEmail()
    {
		SignInHelper.reset();
		SignInResponse signIn = SignIn.endpoint(new SignInBody
		(
			SignUpHelper.getPassword() + SignUpHelper.getEmail(),
			"MSTest",
			SignUpHelper.getPassword()
		));
		Assert.IsTrue(signIn.response == "error","signIn.response was \"" + signIn.response + "\"");
		Assert.IsFalse(signIn.response == "success","signIn.response was \"" + signIn.response + "\"");
	}

	[TestMethod]
    public void FailOnPassword()
    {
		SignInHelper.reset();
		SignInResponse signIn = SignIn.endpoint(new SignInBody
		(
			SignUpHelper.getEmail(),
			"MSTest",
			SignUpHelper.getPassword() + SignUpHelper.getPassword()
		));
		Assert.IsTrue(signIn.response == "error","signIn.response was \"" + signIn.response + "\"");
		Assert.IsFalse(signIn.response == "success","signIn.response was \"" + signIn.response + "\"");
	}
}