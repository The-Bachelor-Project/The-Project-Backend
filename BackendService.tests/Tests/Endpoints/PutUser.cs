namespace BackendService.tests;

[TestClass]
public class PutUserTest
{
	[TestMethod, Priority(0)]
	public void EndpointChangeEmail()
	{
		String newEmail = Tools.RandomString.Generate(10) + "@test.com";
		API.v1.PutEmailBody body = new API.v1.PutEmailBody(Assembly.email, newEmail);
		API.v1.PutUserResponse response = API.v1.PutUsers.EndpointEmail(Assembly.accessToken, body);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assembly.email = newEmail;
	}

	[TestMethod, Priority(0)]
	public void EndpointChangePass()
	{
		String newPass = "aB!" + Tools.RandomString.Generate(10);
		API.v1.PutPasswordBody body = new API.v1.PutPasswordBody(Assembly.password, newPass, Assembly.email);
		API.v1.PutUserResponse response = API.v1.PutUsers.EndpointPass(Assembly.accessToken, body);
		Assert.IsTrue(response.response == "success", "response should be \"success\" but was " + response.response);
		Assembly.password = newPass;
	}
}