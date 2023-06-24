namespace Tools;
using System.Data.SqlClient;
public class ValidCurrency
{
	public static Boolean Check(String currency)
	{
		String getCurrencyQuery = "SELECT * FROM Currencies WHERE code = @code";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@code", currency);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getCurrencyQuery, parameters);
		if (data != null)
		{
			return true;
		}
		return false;
	}
}