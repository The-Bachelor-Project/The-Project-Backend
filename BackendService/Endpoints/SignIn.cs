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
			Authentication.GottenTokenResponse GottenRefreshToken = Authentication.GetToken.RefreshToken(UserID);
			if (GottenRefreshToken.Success)
			{
				Authentication.TokenGeneration.AccessToken(GottenRefreshToken.Token);
				signInResponse.response = "success";
				signInResponse.refreshToken = GottenRefreshToken.Token;
				signInResponse.uid = UserID;
			}
			else
			{
				signInResponse.response = "error getting refresh token";
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
	public SignInResponse(string response, string refreshToken, string uid)
	{
		this.response = response;
		this.refreshToken = refreshToken;
		this.uid = uid;
	}

	public String response { get; set; }
	public String refreshToken { get; set; }
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