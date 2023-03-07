using System.Data.SqlClient;

namespace BackendService;

class SignIn
{
	public static SignInResponse endpoint(SignInBody body)
	{
		SignInResponse signInResponse = new SignInResponse("error", "nope");

		try
		{
			String UserID = DatabaseService.User.SignIn(body.email, body.password);
			Authentication.GrantTokenResponse GrantTokenResponse = Authentication.GetToken.GrantToken(UserID);
			if (GrantTokenResponse.Success)
			{
				Authentication.TokenGeneration.RefreshToken(GrantTokenResponse.GrantToken);
				signInResponse.response = "success";
			}
			else
			{
				signInResponse.response = "error getting grant token";
			}

		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			signInResponse.response = "error";
		}

		return signInResponse;
	}


}

class SignInResponse
{
	public SignInResponse(string response, string token)
	{
		this.response = response;
		this.token = token;
	}

	public String response { get; set; }
	public String token { get; set; }
}

class SignInBody
{
	public SignInBody(string email, string device, string password)
	{
		this.email = email;
		this.device = device;
		this.password = password;
	}

	public String email { get; set; }
	public String? device { get; set; }
	public String password { get; set; }
}