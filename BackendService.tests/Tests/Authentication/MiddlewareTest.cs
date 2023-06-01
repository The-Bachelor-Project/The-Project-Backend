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
	public async Task MiddlewareTest_IgnoredPathSignInTest()
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
	public async Task MiddlewareTest_IgnoredPathSignUpTest()
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
	public async Task MiddlewareTest_ValidAccessTokenAndCommonPathSuccessTest()
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
	public async Task MiddlewareTest_EmptyAccessTokenAndCommonPathTest()
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
	public async Task MiddlewareTest_ValidRefreshTokenAndRefreshTokenPathTest()
	{
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "PUT";
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
	public async Task MiddlewareTest_ExpiredAccessTokenAndCommonPathTest()
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
	public async Task MiddlewareTest_ExpiredRefreshTokenAndTokenRefreshPathTest()
	{
		TokenHelper.MakeTokensExpired(userTestObject.accessToken!, userTestObject.refreshToken!);
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "PUT";
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
	public async Task MiddlewareTest_InvalidAccessTokenAndCommonPathTest()
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
	public async Task MiddlwareTest_InvalidRefreshTokenAndRefreshPathTest()
	{
		TokenHelper.CreateTokens(userTestObject.user!, (int)userTestObject.familyID!);
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "PUT";
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

	[TestMethod]
	public async Task MiddlewareTest_MissingRefreshTokenTest()
	{
		TokenHelper.CreateTokens(userTestObject.user!, (int)userTestObject.familyID!);
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.Path = "/v1/tokens";
		context.Request.Method = "PUT";
		RequestDelegate next = (HttpContext context) =>
		{
			return Task.CompletedTask;
		};
		Authentication.Middleware middleware = new Authentication.Middleware();
		await middleware.InvokeAsync(context, next);
		Assert.AreEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
	}
}
