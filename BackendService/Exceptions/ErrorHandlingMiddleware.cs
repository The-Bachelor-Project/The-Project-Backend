public class ErrorHandlingMiddlware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (StatusCodeException e)
		{
			context.Response.StatusCode = e.StatusCode;
			await context.Response.WriteAsync(e.Message);
		}
		catch (Exception e)
		{
			context.Response.StatusCode = 500;
			await context.Response.WriteAsync(e.Message);
		}
	}
}