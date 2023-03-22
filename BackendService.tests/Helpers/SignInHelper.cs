using BackendService;
using Tools;

class SignInHelper{
	static string email = "";
	static string password = "";

	private static void signUp(){
		Thread thread = new Thread(Program.Main);
		thread.Start();

		string random = RandomString.Generate(32);
		email = random + "@test.mail";
		password = RandomString.Generate(16);

		SignUpResponse signUp = SignUp.endpoint(new SignUpBody
		(
			email,
			password
		));
	}

	public static string getEmail(){
		if(email.Length == 0){
			signUp();
		}
		return email;
	}

	public static string getPassword(){
		if(password.Length == 0){
			signUp();
		}
		return password;
	}
}