using System.Data.SqlClient;

namespace Data.Database;

class Exchange
{
	public static String GetCurrency(String exchange)
	{
		String getCurrencyQuery = "SELECT currency FROM Exchanges WHERE symbol = @symbol";
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand(getCurrencyQuery, connection);
		command.Parameters.AddWithValue("@symbol", exchange);
		SqlDataReader reader = command.ExecuteReader();
		String currency = "";
		if (reader.Read())
		{
			currency = reader["currency"].ToString()!;
		}
		reader.Close();
		return currency;
	}
}