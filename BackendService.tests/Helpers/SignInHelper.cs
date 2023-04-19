using BackendService;
using Tools;
using API.v1;
namespace BackendService.tests;

class SignInHelper
{
	static string RefreshToken = "";
	static string AccessToken = "";

	private static void signIn()
	{
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.getEmail(),
			SignUpHelper.getPassword()
		));
		RefreshToken = SignIn.tokenSet.RefreshToken!;
		AccessToken = SignIn.tokenSet.AccessToken!;
	}

	public static string getEmail()
	{
		return SignUpHelper.getEmail();
	}

	public static string getPassword()
	{
		return SignUpHelper.getPassword();
	}

	public static string getRefreshToken()
	{
		if (RefreshToken == "")
		{
			signIn();
		}
		return RefreshToken;
	}

	public static String GetAccessToken()
	{
		if (AccessToken == "")
		{
			signIn();
		}
		return AccessToken;
	}

	public static void setRefreshToken(string newToken)
	{
		RefreshToken = newToken;
	}

	public static void setAccessToken(string newToken)
	{
		AccessToken = newToken;
	}

	public static void reset()
	{
		RefreshToken = "";
		AccessToken = "";
		SignUpHelper.reset();
	}
}