namespace Authentication;
public class Middleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if ((context.Request.Path == "/v1/tokens" && context.Request.Method == "POST") || ((context.Request.Path == "/v1/users" && context.Request.Method == "POST")))
		{
			await next(context);
			return;
		}

		if (context.Request.Path == "/v1/tokens" && context.Request.Method == "PUT")
		{
			String? refreshToken = context.Request.Headers["Authorization"];
			if (refreshToken != null)
			{
				RefreshAuthenticationResponse response = Authentication.Authenticate.RefreshToken(refreshToken);
				if (response.error == "Invalid")
				{
					context.Response.StatusCode = 401;
					await context.Response.WriteAsync("Refresh token is invalid");
					return;
				}
				else if (response.error == "Expired")
				{
					context.Response.StatusCode = 403;
					await context.Response.WriteAsync("Refresh token is expired");
					return;
				}
				else
				{
					context.Items["RefreshToken"] = refreshToken;
					context.Items["FamilyID"] = response.familyID;
					await next(context);
					return;
				}
			}
			else
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("No token was provided");
				return;
			}
		}

		String? accessToken = context.Request.Headers["Authorization"];
		if (accessToken != null)
		{
			String isValid = Authentication.Authenticate.AccessToken(accessToken);
			if (isValid == "Invalid")
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("Access token is invalid");
				return;
			}
			else if (isValid == "Expired")
			{
				context.Response.StatusCode = 403;
				await context.Response.WriteAsync("Access token has expired");
				return;
			}
			else
			{
				context.Items["AccessToken"] = accessToken;
				await next(context);
				return;
			}
		}
		else
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("No token was provided");
			return;
		}
	}
}