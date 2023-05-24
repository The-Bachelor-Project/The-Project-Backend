using StockApp;

namespace API.v1;

public class PostTokens
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/tokens", (PostTokensBody body) =>
		{
			return Results.Ok(Endpoint(body));
		});
	}

	public static TokensResponse Endpoint(PostTokensBody body)
	{
		try
		{
			TokenSet newTokenSet = TokenSet.Create(new User(body.email, body.password).SignIn().id!);
			return new TokensResponse("success", newTokenSet);
		}
		catch (DatabaseException e)
		{
			System.Console.WriteLine(e.Message);
			TokensResponse response = new TokensResponse("error", new TokenSet());
			response.error = "Something happened - try again";
			return response;
		}
		catch (WrongPasswordException e)
		{
			System.Console.WriteLine(e.Message);
			TokensResponse response = new TokensResponse("error", new TokenSet());
			response.error = e.Message;
			return response;
		}
		catch (UserDoesNotExistException e)
		{
			System.Console.WriteLine(e.Message);
			TokensResponse response = new TokensResponse("error", new TokenSet());
			response.error = e.Message;
			return response;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e.Message);
			TokensResponse response = new TokensResponse("error", new TokenSet());
			response.error = "Unknown error - please try again";
			return response;
		}
	}
}
