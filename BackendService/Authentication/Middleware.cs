namespace Authentication;
class Middleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (context.Request.Path == "/v1/tokens" && context.Request.Method == "POST" || (context.Request.Path == "/v1/users" && context.Request.Method == "POST"))
		{
			await next(context);
			return;
		}

		if (context.Request.Path == "/v1/tokens" && context.Request.Method == "GET")
		{
			// TODO: Maybe check for authentication with the refreshtoken here, instead of on the endpoint
			await next(context);
			return;
		}

		String? accessToken = context.Request.Headers["Authorization"];
		if (accessToken != null)
		{
			if (!Authentication.Authenticate.AccessToken(accessToken))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("Access token is invalid.");
				return;
			}
		}
		else
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("No access token provided for authentication.");
			return;
		}
		context.Items["AccessToken"] = accessToken;
		await next(context);
	}
}