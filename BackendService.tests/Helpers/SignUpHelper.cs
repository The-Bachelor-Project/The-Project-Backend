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
		StockApp.User user = new StockApp.User(email, password).SignUp();
	}

	public static string GetEmail()
	{
		if (email == "")
		{
			SignUp();
		}
		return email;
	}

	public static string GetPassword()
	{
		if (password == "")
		{
			SignUp();
		}
		return password;
	}

	public static void Reset()
	{
		email = "";
		password = "";
	}
}