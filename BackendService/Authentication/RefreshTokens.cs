using System.Data.SqlClient;

namespace Authentication;

public class RefreshTokens
{
	public static StockApp.TokenSet All(String refreshToken)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String getFamilyID = "SELECT family FROM Tokens WHERE refresh_token = @refresh_token";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@refresh_token", refreshToken);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getFamilyID, parameters);
		if (data != null)
		{
			int familyID = (int)data["family"];
			StockApp.TokenSet tokenSet = SetupTokens.Call(familyID);
			return tokenSet;
		}
		throw new StatusCodeException(401);


	}

}