using StockApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.v1;

public class PostTokens
{
	public static void Setup(WebApplication app)
	{
		app.MapPost("/v1/tokens", (PostTokensBody body) =>
		{
			return Endpoint(body);
		});
	}

	public static TokenSet Endpoint(PostTokensBody body)
	{
		if (body.email == "")
		{
			throw new StatusCodeException(400, "Email cannot be empty");
		}
		else if (body.password == "")
		{
			throw new StatusCodeException(400, "Password cannot be empty");
		}
		TokenSet newTokenSet = TokenSet.Create(new User(body.email, body.password).SignIn().id!);
		return newTokenSet;

	}
}
