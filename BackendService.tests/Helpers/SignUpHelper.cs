using BackendService;
using Tools;

namespace BackendService.tests;

class SignUpHelper{
	static string email = "";
	static string password = "";

	private static void signUp(){
		Backend.Start();

		string random = RandomString.Generate(32);
		email = random + "@test.mail";
		password = "aB1!" + RandomString.Generate(8);

		SignUpResponse signUp = SignUp.endpoint(new SignUpBody
		(
			email,
			password
		));
	}

	public static string getEmail(){
		if(email == ""){
			signUp();
		}
		return email;
	}

	public static string getPassword(){
		if(password == ""){
			signUp();
		}
		return password;
	}

	public static void reset(){
		email = "";
		password = "";
	}
}