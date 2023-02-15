using System.Data.SqlClient;
using BackendService;

namespace DatabaseService;

class StockTagGenerator
{
	public static String generate(Data.StockProfile stockProfile)
	{
		//TODO: Make variants for NOVO-B, NOVO B, AT&T, ATT, AT T, so fourth
		String tags = "";
		tags += stockProfile.Exchange + " " + stockProfile.Ticker + ",";
		tags += stockProfile.Ticker + " " + stockProfile.Exchange + ",";
		tags += stockProfile.Name + ",";
		return tags.ToLower();
	}

	public static void updateAllStocks()
	{
		using (SqlConnection connection = Database.createConnection())
		{
			String query = "SELECT * FROM Stocks";
			SqlCommand command = new SqlCommand(query, connection);
			SqlDataReader reader = command.ExecuteReader();

			String updateQuery = "UPDATE Stocks SET tags = @tags WHERE ticker = @ticker AND exchange = @exchange";
			List<Data.StockProfile> stockProfileList = new List<Data.StockProfile>();
			while (reader.Read())
			{
				Data.StockProfile stockProfile = new Data.StockProfile();
				stockProfile.Country = reader["country"].ToString();
				stockProfile.Exchange = reader["exchange"].ToString();
				stockProfile.Industry = reader["industry"].ToString();
				stockProfile.Name = reader["company_name"].ToString();
				stockProfile.Sector = reader["sector"].ToString();
				stockProfile.Ticker = reader["ticker"].ToString();
				stockProfile.Exchange = reader["exchange"].ToString();
				stockProfile.Website = reader["website"].ToString();
				stockProfileList.Add(stockProfile);
			}
			reader.Close();
			foreach (Data.StockProfile stockProfile in stockProfileList)
			{
				SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
				updateCommand.Parameters.AddWithValue("@tags", generate(stockProfile));
				updateCommand.Parameters.AddWithValue("@ticker", stockProfile.Ticker);
				updateCommand.Parameters.AddWithValue("@exchange", stockProfile.Exchange);
				updateCommand.ExecuteNonQuery();
			}
		}
	}
}