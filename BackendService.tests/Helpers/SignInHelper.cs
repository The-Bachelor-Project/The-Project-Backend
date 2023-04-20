using BackendService;
using Tools;
using API.v1;
namespace BackendService.tests;

class SignInHelper
{
	static string RefreshToken = "";
	static string AccessToken = "";

	private static void SignIn()
	{
		TokensResponse SignIn = PostTokens.Endpoint(new PostTokensBody
		(
			SignUpHelper.GetEmail(),
			SignUpHelper.GetPassword()
		));
		RefreshToken = SignIn.tokenSet.refreshToken!;
		AccessToken = SignIn.tokenSet.accessToken!;
	}

	public static string GetEmail()
	{
		return SignUpHelper.GetEmail();
	}

	public static string GetPassword()
	{
		return SignUpHelper.GetPassword();
	}

	public static string GetRefreshToken()
	{
		if (RefreshToken == "")
		{
			SignIn();
		}
		return RefreshToken;
	}

	public static String GetAccessToken()
	{
		if (AccessToken == "")
		{
			SignIn();
		}
		return AccessToken;
	}

	public static void SetRefreshToken(string newToken)
	{
		RefreshToken = newToken;
	}

	public static void SetAccessToken(string newToken)
	{
		AccessToken = newToken;
	}

	public static void Reset()
	{
		RefreshToken = "";
		AccessToken = "";
		SignUpHelper.Reset();
	}
}