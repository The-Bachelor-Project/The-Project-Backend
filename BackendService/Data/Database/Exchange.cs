using System.Data.SqlClient;

namespace Data.Database;

class Exchange
{
	public static String GetCurrency(String exchange)
	{
		String GetCurrencyQuery = "SELECT currency FROM Exchanges WHERE symbol = @symbol";
		SqlConnection Connection = new Data.Database.Connection().Create();
		SqlCommand Command = new SqlCommand(GetCurrencyQuery, Connection);
		Command.Parameters.AddWithValue("@symbol", exchange);
		SqlDataReader Reader = Command.ExecuteReader();
		if (Reader.Read())
		{
			return Reader["currency"].ToString()!;
		}
		else
		{
			return "";
		}
	}
}