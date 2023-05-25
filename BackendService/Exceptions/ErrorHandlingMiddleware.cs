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
		catch (UserAlreadyExist e)
		{
			context.Response.StatusCode = 409;
			await context.Response.WriteAsync(e.Message);
		}
		catch (InvalidUserInput e)
		{
			context.Response.StatusCode = 400;
			await context.Response.WriteAsync(e.Message);
		}
		catch (CouldNotGetStockException e)
		{
			context.Response.StatusCode = 404;
			await context.Response.WriteAsync(e.Message);
		}
		catch (CurrencyLookupException e)
		{
			context.Response.StatusCode = 404;
			await context.Response.WriteAsync(e.Message);
		}
		catch (CurrencyHistoryException e)
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