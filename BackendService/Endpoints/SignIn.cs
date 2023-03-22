using System.Data.SqlClient;

namespace BackendService;

public class SignIn
{
	public static SignInResponse endpoint(SignInBody body)
	{
		SignInResponse signInResponse = new SignInResponse("error", "nope", "");

		try
		{
			String UserID = DatabaseService.User.SignIn(body.email, body.password);
			Authentication.GottenTokenResponse GottenGrantToken = Authentication.GetToken.RefreshToken(UserID);
			if (GottenGrantToken.Success)
			{
				Authentication.TokenGeneration.AccessToken(GottenGrantToken.Token);
				signInResponse.response = "success";
				signInResponse.token = GottenGrantToken.Token;
				signInResponse.uid = UserID;
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

public class SignInResponse
{
	public SignInResponse(string response, string token, string uid)
	{
		this.response = response;
		this.token = token;
		this.uid = uid;
	}

	public String response { get; set; }
	public String token { get; set; }
	public String uid { get; set; }
}

public class SignInBody
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