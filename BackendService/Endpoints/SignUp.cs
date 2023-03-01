using System.Data.SqlClient;

namespace BackendService;

class SignUp
{
	public static SignUpResponse endpoint(SignUpBody body)
	{
		SignUpResponse signUpResponse = new SignUpResponse("error", "i dunno");

		try
		{
			signUpResponse.uid = DatabaseService.User.SignUp(body.email, body.password);

			signUpResponse.response = "success";
		}
		catch (Exception e)
		{
			signUpResponse.response = "Email already in use";
			System.Console.WriteLine(e);
		}



		return signUpResponse;
	}
}

class SignUpResponse
{
	public SignUpResponse(string response, string uid)
	{
		this.response = response;
		this.uid = uid;
	}

	public String response { get; set; }
	public String uid { get; set; }
}

class SignUpBody
{
	public SignUpBody(string email, string password)
	{
		this.email = email;
		this.password = password;
	}

	public String email { get; set; }
	public String password { get; set; }
}