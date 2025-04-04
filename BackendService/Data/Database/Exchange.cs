using System.Data.SqlClient;

namespace Data.Database;

public class Exchange
{
	public static String GetCurrency(String exchange)
	{
		String getCurrencyQuery = "SELECT currency FROM Exchanges WHERE symbol = @symbol";

		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@symbol", exchange);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getCurrencyQuery, parameters);
		String currency = "";
		if (data != null)
		{
			currency = data["currency"].ToString()!;
			return currency;
		}
		else
		{
			throw new StatusCodeException(404, "Currency of exchange " + exchange + " not found");
		}
	}
}