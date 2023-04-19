using BackendService;
using Tools;
using API.v1;
namespace BackendService.tests;

class SignUpHelper
{
	static string email = "";
	static string password = "";

	private static void SignUp()
	{
		Backend.Start();

		string random = RandomString.Generate(32);
		email = random + "@test.mail";
		password = "aB1!" + RandomString.Generate(8);

		PostUsersResponse SignUp = PostUsers.Endpoint(new PostUsersBody
		(
			email,
			password
		));
	}

	public static string getEmail()
	{
		if (email == "")
		{
			SignUp();
		}
		return email;
	}

	public static string getPassword()
	{
		if (password == "")
		{
			SignUp();
		}
		return password;
	}

	public static void reset()
	{
		email = "";
		password = "";
	}
}