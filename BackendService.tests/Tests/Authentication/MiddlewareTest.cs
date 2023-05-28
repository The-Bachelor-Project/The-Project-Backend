namespace BackendService.tests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

[TestClass]
public class MiddlewareTest
{
	private static UserTestObject userTestObject = new UserTestObject();

	[TestInitialize]
	public void Initialize()
	{
		userTestObject = UserHelper.Create();
	}

	[TestCleanup]
	public void Cleanup()
	{
		UserHelper.Delete(userTestObject);
	}

	[TestMethod]
	public async Task MiddlewareTest_IgnoredPathSignIn()
	{
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "POST";
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status200OK, context.Response.StatusCode);
	}

	[TestMethod]
	public async Task MiddlewareTest_IgnoredPathSignUp()
	{
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/users";
		context.Request.Method = "POST";
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status200OK, context.Response.StatusCode);
	}

	[TestMethod]
	public async Task MiddlewareTest_ValidAccessTokenAndCommonPathSuccess()
	{
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/portfolios";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = userTestObject.accessToken;
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status200OK, context.Response.StatusCode);
	}

	[TestMethod]
	public async Task MiddlewareTest_EmptyAccessTokenAndCommonPath()
	{
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/portfolios";
		context.Request.Method = "GET";
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
	}

	[TestMethod]
	public async Task MiddlewareTest_ValidRefreshTokenAndRefreshTokenPath()
	{
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = userTestObject.refreshToken;
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status200OK, context.Response.StatusCode);
	}

	[TestMethod]
	public async Task MiddlewareTest_ExpiredAccessTokenAndCommonPath()
	{
		TokenHelper.MakeTokensExpired(userTestObject.accessToken!, userTestObject.refreshToken!);
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/portfolios";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = userTestObject.accessToken;
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status403Forbidden, context.Response.StatusCode);
	}
	[TestMethod]
	public async Task MiddlewareTest_ExpiredRefreshTokenAndTokenRefreshPath()
	{
		TokenHelper.MakeTokensExpired(userTestObject.accessToken!, userTestObject.refreshToken!);
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = userTestObject.refreshToken;
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status403Forbidden, context.Response.StatusCode);
	}

	[TestMethod]
	public async Task MiddlewareTest_InvalidAccessTokenAndCommonPath()
	{
		TokenHelper.CreateTokens(userTestObject.user!, (int)userTestObject.familyID!);
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/portfolios";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = userTestObject.accessToken;
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
		Assert.IsTrue(TokenHelper.InvalidatedFamilyCorrectly((int)userTestObject.familyID!));
	}
	[TestMethod]
	public async Task MiddlwareTest_InvalidRefreshTokenAndRefreshPath()
	{
		TokenHelper.CreateTokens(userTestObject.user!, (int)userTestObject.familyID!);
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "GET";
		context.Request.Headers["Authorization"] = userTestObject.refreshToken;
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
		Assert.IsTrue(TokenHelper.InvalidatedFamilyCorrectly((int)userTestObject.familyID!));
	}
}
