namespace BackendService.tests;
using Microsoft.AspNetCore.Http;

[TestClass]
public class ExceptionsTest
{
	[TestMethod]
	public void ExceptionsTest_StatusCodeExceptionCreationTest()
	{
		StatusCodeException statusException = new StatusCodeException(400, "test");
		Assert.IsTrue(statusException.Message == "test", "statusException.Message should be \"test\" but was \"" + statusException.Message + "\"");
		Assert.IsTrue(statusException.StatusCode == 400, "statusException.Status should be 400 but was " + statusException.StatusCode);
	}

	[TestMethod]
	public void ExceptionsTest_StatusCodeExceptionThrowTest()
	{
		try
		{
			throw new StatusCodeException(400, "test");
		}
		catch (StatusCodeException e)
		{
			Assert.IsTrue(e.Message == "test", "error message should be \"test\" but was \"" + e.Message + "\"");
			Assert.IsTrue(e.StatusCode == 400, "error status code should be 400 but was " + e.StatusCode);
		}
	}

	[TestMethod]
	public void ExceptionsTest_ErrorHandlingMiddlewareStatusCodeExceptionTest()
	{
		ErrorHandlingMiddlware middlware = new ErrorHandlingMiddlware();
		DefaultHttpContext context = new DefaultHttpContext();
		context.Response.Body = new MemoryStream();
		StatusCodeException exception = new StatusCodeException(409, "Test");
		middlware.InvokeAsync(context, (c) => throw exception).Wait();
		Assert.IsTrue(context.Response.StatusCode == 409, "status code on response should be 409 but was " + context.Response.StatusCode);
	}

	[TestMethod]
	public void ExceptionsTest_ErrorHandlingMiddlewareOtherExceptionTest()
	{
		ErrorHandlingMiddlware middlware = new ErrorHandlingMiddlware();
		DefaultHttpContext context = new DefaultHttpContext();
		context.Response.Body = new MemoryStream();
		Exception exception = new Exception("Test");
		middlware.InvokeAsync(context, (c) => throw exception).Wait();
		Assert.IsTrue(context.Response.StatusCode == 500, "status code on response should be 500 but was " + context.Response.StatusCode);
	}
}