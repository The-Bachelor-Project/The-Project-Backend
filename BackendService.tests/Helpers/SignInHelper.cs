using BackendService;
using Tools;

namespace BackendService.tests;

class SignInHelper{
	static string token = "";

	private static void signIn(){
		SignInResponse signIn = SignIn.endpoint(new SignInBody
		(
			getEmail(),
			"MSTest",
			getPassword()
		));
		token = signIn.refreshToken;
	}

	public static string getEmail(){
		return SignUpHelper.getEmail();
	}

	public static string getPassword(){
		return SignUpHelper.getPassword();
	}

	public static string getRefreshToken(){
		if(token == ""){
			signIn();
		}
		return token;
	}

	public static void setRefreshToken(string newToken){
		token = newToken;
	}

	public static void reset(){
		token = "";
		SignUpHelper.reset();
	}
}