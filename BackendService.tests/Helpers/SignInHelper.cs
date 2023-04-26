using BackendService;
using Tools;
using API.v1;
namespace BackendService.tests;

class SignInHelper
{
	static string refreshToken = "";
	static string accessToken = "";

	private static void SignIn()
	{
		StockApp.TokenSet tokenSet = StockApp.TokenSet.Create(new StockApp.User(SignUpHelper.GetEmail(), SignUpHelper.GetPassword()).SignIn().id!);
		refreshToken = tokenSet.refreshToken!;
		accessToken = tokenSet.accessToken!;
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
		if (refreshToken == "")
		{
			SignIn();
		}
		return refreshToken;
	}

	public static String GetAccessToken()
	{
		if (accessToken == "")
		{
			SignIn();
		}
		return accessToken;
	}

	public static void SetRefreshToken(string newToken)
	{
		refreshToken = newToken;
	}

	public static void SetAccessToken(string newToken)
	{
		accessToken = newToken;
	}

	public static void Reset()
	{
		refreshToken = "";
		accessToken = "";
		SignUpHelper.Reset();
	}
}