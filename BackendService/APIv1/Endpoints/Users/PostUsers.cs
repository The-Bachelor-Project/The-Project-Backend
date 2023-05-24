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
		try
		{
			User newUser = new User(body.email, body.password).SignUp();
			return new PostUsersResponse("success", newUser.id!);
		}
		catch (UserAlreadyExist e)
		{
			System.Console.WriteLine(e.Message);
			PostUsersResponse response = new PostUsersResponse("error", "");
			response.error = e.Message;
			return response;
		}
		catch (DatabaseException e)
		{
			System.Console.WriteLine(e.Message);
			PostUsersResponse response = new PostUsersResponse("error", "");
			response.error = "Something weird happened - please try again";
			return response;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e.Message);
			PostUsersResponse response = new PostUsersResponse("error", "");
			response.error = "Unknown error - please try again";
			return response;
		}

	}
}
