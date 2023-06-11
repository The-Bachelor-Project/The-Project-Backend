using StockApp;
namespace API.v1;
using System.Data.SqlClient;

public class PutUsers
{
	public static void Setup(WebApplication app)
	{
		app.MapPut("/v1/users/email", (HttpContext context, PutEmailBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(EndpointEmail(accessToken!, body));
		});

		app.MapPut("/v1/users/password", (HttpContext context, PutPasswordBody body) =>
		{
			String? accessToken = context.Items["AccessToken"] as String;
			return Results.Ok(EndpointPass(accessToken!, body));
		});
	}

	public static PutUserResponse EndpointPass(string accessToken, PutPasswordBody body)
	{
		if (body.oldPassword is null || body.newPassword is null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		User user = new TokenSet(accessToken).GetUser();
		user.ChangePassword(body.oldPassword, body.newPassword);
		return new PutUserResponse("success");

	}

	public static PutUserResponse EndpointEmail(String accessToken, PutEmailBody body)
	{
		if (body.newEmail is null || body.password is null)
		{
			throw new StatusCodeException(400, "Missing required fields");
		}
		if (!Tools.ValidEmail.Check(body.newEmail))
		{
			throw new StatusCodeException(400, "Invalid email");
		}
		User user = new TokenSet(accessToken).GetUser();
		user.ChangeEmail(body.newEmail);
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String changePasswordQuery = "UPDATE Accounts SET password = @password WHERE id = @id";
		SqlCommand command = new SqlCommand(changePasswordQuery, connection);
		command.Parameters.AddWithValue("@password", Tools.Password.Hash(body.password, user.id!));
		command.Parameters.AddWithValue("@id", user.id!);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to change email");
		}
		return new PutUserResponse("success");
	}
}