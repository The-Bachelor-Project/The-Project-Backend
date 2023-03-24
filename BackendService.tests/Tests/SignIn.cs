using BackendService;

namespace BackendService.tests;

[TestClass]
public class SignInTest
{
	[TestMethod]
    public void Succeed()
    {
		SignInResponse signIn = SignIn.endpoint(new SignInBody
		(
			SignInHelper.getEmail(),
			"MSTest",
			SignInHelper.getPassword()
		));
		Assert.IsFalse(signIn.response == "error","signIn.response was \"" + signIn.response + "\"");
		Assert.IsTrue(signIn.response == "success","signIn.response was \"" + signIn.response + "\"");
	}

	[TestMethod]
    public void FailOnEmail()
    {
		SignInResponse signIn = SignIn.endpoint(new SignInBody
		(
			SignInHelper.getPassword() + SignInHelper.getEmail(),
			"MSTest",
			SignInHelper.getPassword()
		));
		Assert.IsTrue(signIn.response == "error","signIn.response was \"" + signIn.response + "\"");
		Assert.IsFalse(signIn.response == "success","signIn.response was \"" + signIn.response + "\"");
	}

	[TestMethod]
    public void FailOnPassword()
    {
		SignInResponse signIn = SignIn.endpoint(new SignInBody
		(
			SignInHelper.getEmail(),
			"MSTest",
			SignInHelper.getPassword() + SignInHelper.getPassword()
		));
		Assert.IsTrue(signIn.response == "error","signIn.response was \"" + signIn.response + "\"");
		Assert.IsFalse(signIn.response == "success","signIn.response was \"" + signIn.response + "\"");
	}
}