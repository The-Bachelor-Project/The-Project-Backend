using Microsoft.AspNetCore.Http;
using System.Web;

namespace BackendService.tests;

[TestClass]
public class MiddleWareTest
{
	[TestMethod]
	public async Task AuthenticateValidTokenTest()
	{
		HttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/endpoint";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = Assembly.accessToken;

		Authentication.Middleware middleware = new Authentication.Middleware();

		await middleware.InvokeAsync(context, (ctx) => Task.FromResult(0));

		Assert.IsTrue(context.Response.StatusCode == 200, "statuscode should be 200 but was " + context.Response.StatusCode);

	}

	[TestMethod]
	public async Task AuthenticateInvalidTokenTest()
	{
		HttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/endpoint";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = "NOTCORRECTTOKEN";

		Authentication.Middleware middleware = new Authentication.Middleware();

		await middleware.InvokeAsync(context, (ctx) => Task.FromResult(0));

		Assert.IsTrue(context.Response.StatusCode == 401, "statuscode should be 401 but was " + context.Response.StatusCode);
	}

	[TestMethod]
	public async Task AuthenticateNoToken()
	{
		HttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/endpoint";
		context.Request.Method = "GET";

		Authentication.Middleware middleware = new Authentication.Middleware();

		await middleware.InvokeAsync(context, (ctx) => Task.FromResult(0));

		Assert.IsTrue(context.Response.StatusCode == 401, "statuscode should be 401 but was " + context.Response.StatusCode);
	}
}