using StockApp;
namespace API.v1;

public class PutUsers
{
	public static void Setup(WebApplication app)
	{
		app.MapPut("/v1/users/email", (HttpContext context, ChangeEmailBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new PutUserResponse("error"));
			}
			return Results.Ok(EndpointEmail(accessToken, body));
		});

		app.MapPut("/v1/users/password", (HttpContext context, ChangePasswordBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			if (accessToken is null)
			{
				context.Response.StatusCode = 401;
				return Results.Ok(new PutUserResponse("error"));
			}
			return Results.Ok(EndpointPass(accessToken, body));
		});
	}

	private static PutUserResponse EndpointPass(string accessToken, ChangePasswordBody body)
	{
		if (body.oldPassword is null || body.newPassword is null || body.email is null)
		{
			return new PutUserResponse("error");
		}
		try
		{
			User user = new TokenSet(accessToken).GetUser();
			user.ChangePassword(body.oldPassword, body.newPassword, body.email!);
			return new PutUserResponse("success");
		}
		catch (System.Exception)
		{
			return new PutUserResponse("error");
		}
	}

	public static PutUserResponse EndpointEmail(String accessToken, ChangeEmailBody body)
	{
		if (body.oldEmail is null || body.newEmail is null)
		{
			return new PutUserResponse("error");
		}
		try
		{
			User user = new TokenSet(accessToken).GetUser();
			user.ChangeEmail(body.oldEmail, body.newEmail);
			return new PutUserResponse("success");
		}
		catch (Exception)
		{
			return new PutUserResponse("error");
		}
	}
}