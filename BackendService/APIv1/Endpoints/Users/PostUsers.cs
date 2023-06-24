using StockApp;

namespace API.v1;

public class PostUsers
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/users", (PostUsersBody body) =>
		{
			return Results.Ok(Endpoint(body));
		});
	}

	public static PostUsersResponse Endpoint(PostUsersBody body)
	{
		if (!Tools.ValidEmail.Check(body.email))
		{
			throw new StatusCodeException(400, "Invalid email");
		}
		User newUser = new User(body.email, body.password).SignUp();
		return new PostUsersResponse("success", newUser.id!);

	}
}
