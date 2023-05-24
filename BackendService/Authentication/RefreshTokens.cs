using System.Data.SqlClient;

namespace Authentication;

public class RefreshTokens
{
	public static StockApp.TokenSet All(String refreshToken, int familyID)
	{

		StockApp.TokenSet tokenSet = SetupTokens.Call(familyID);
		return tokenSet;
	}

}