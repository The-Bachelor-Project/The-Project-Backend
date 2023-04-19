using BackendService;
using Tools;
using API.v1;
namespace BackendService.tests;

class SignUpHelper
{
	static string Email = "";
	static string Password = "";

	private static void SignUp()
	{
		Backend.Start();

		string Random = RandomString.Generate(32);
		Email = Random + "@test.mail";
		Password = "aB1!" + RandomString.Generate(8);

		PostUsersResponse SignUp = PostUsers.Endpoint(new PostUsersBody
		(
			Email,
			Password
		));
	}

	public static string GetEmail()
	{
		if (Email == "")
		{
			SignUp();
		}
		return Email;
	}

	public static string GetPassword()
	{
		if (Password == "")
		{
			SignUp();
		}
		return Password;
	}

	public static void Reset()
	{
		Email = "";
		Password = "";
	}
}