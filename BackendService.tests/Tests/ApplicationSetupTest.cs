using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace BackendService.tests;

[TestClass]
public class ApplicationSetupTest
{
	[TestMethod]
	public void SetupApplicationTest()
	{
		try
		{
			API.v1.Application.Setup(false);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Application could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PostUsersTest()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PostUsers.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}
}