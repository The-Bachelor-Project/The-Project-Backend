public class ErrorHandlingMiddlware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (WrongPasswordException e)
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync(e.Message);
		}
		catch (UserDoesNotExistException e)
		{
			context.Response.StatusCode = 404;
			await context.Response.WriteAsync(e.Message);
		}
		catch (DatabaseException e)
		{
			context.Response.StatusCode = 500;
			await context.Response.WriteAsync(e.Message);
		}
		catch (Exception e)
		{
			context.Response.StatusCode = 500;
			await context.Response.WriteAsync(e.Message);
		}
	}
}