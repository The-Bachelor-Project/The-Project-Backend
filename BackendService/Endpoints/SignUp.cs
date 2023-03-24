using System.Data.SqlClient;

namespace BackendService;

public class SignUp
{
	public static SignUpResponse endpoint(SignUpBody body)
	{
		SignUpResponse signUpResponse = new SignUpResponse("error", "i dunno");

		try
		{
			String UID = DatabaseService.User.SignUp(body.email, body.password);
			String RefreshToken = Tools.RandomString.Generate(128);
			Boolean SuccessfulRefreshCreation = Authentication.TokenGeneration.RefreshToken(UID, RefreshToken);
			Boolean SuccessfulAccessCreation = Authentication.TokenGeneration.AccessToken(RefreshToken);
			if (!SuccessfulRefreshCreation && !SuccessfulAccessCreation)
			{
				throw new IOException();
			}
			signUpResponse.uid = UID;
			signUpResponse.response = "success";
		}
		catch (IOException)
		{
			signUpResponse.response = "Problem with creating refresh and/or refresh token";
		}
		catch (Exception e)
		{
			signUpResponse.response = "Email already in use";
			System.Console.WriteLine(e);
		}

		return signUpResponse;
	}
}

public class SignUpResponse
{
	public SignUpResponse(string response, string uid)
	{
		this.response = response;
		this.uid = uid;
	}

	public String response { get; set; }
	public String uid { get; set; }
}

public class SignUpBody
{
	public SignUpBody(string email, string password)
	{
		this.email = email;
		this.password = password;
	}

	public String email { get; set; }
	public String password { get; set; }
}