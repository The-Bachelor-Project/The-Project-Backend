using Microsoft.AspNetCore.Mvc;

namespace API.v1;

public class GetUsers
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/users", ([FromQuery] string accessToken) =>
		{
			return Results.Ok(Endpoint(accessToken));
		});
	}

	public static GetUsersResponse Endpoint(string accessToken)
	{
		return new GetUsersResponse("success", "4420");
	}
}
