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
		Assert.IsFalse(signIn.response == "error","SignIn failed");
		Assert.IsTrue(signIn.response == "success","SignIn failed");
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
		Assert.IsTrue(signIn.response == "error","SignIn succeed");
		Assert.IsFalse(signIn.response == "success","SignIn failed");
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
		Assert.IsTrue(signIn.response == "error","SignIn succeed");
		Assert.IsFalse(signIn.response == "success","SignIn failed");
	}
}