using System.Data.SqlClient;

namespace Authentication;

public class RefreshTokens
{
	public static StockApp.TokenSet All(String refreshToken)
	{
		ValidFunctionResponse validFunctionResponse = Authenticate.RefreshToken(refreshToken);
		if (validFunctionResponse.isValid == 1)
		{
			StockApp.TokenSet tokenSet = SetupTokens.Call(validFunctionResponse.userID, validFunctionResponse.familyID);
			return tokenSet;
		}
		else if (validFunctionResponse.isValid == 0)
		{
			StockApp.TokenSet tokenSet = new StockApp.TokenSet("is_expired", "");
			return tokenSet;
		}
		else
		{
			StockApp.TokenSet tokenSet = new StockApp.TokenSet("error", "");
			return tokenSet;
		}
	}

}