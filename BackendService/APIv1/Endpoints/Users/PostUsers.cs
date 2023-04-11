using BusinessLogic;

namespace API.v1;

class PostUsers
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
		User newUser = new User(body.email, body.password).SignUp();
		return new PostUsersResponse("success", newUser.Id!);
	}
}
