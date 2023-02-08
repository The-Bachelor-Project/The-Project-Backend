using System.Data.SqlClient;
using BackendService;

class StockTagGenerator
{
	public static String generate(StockInfo stockInfo)
	{
		//TODO: Make variants for NOVO-B, NOVO B, AT&T, ATT, AT T, so fourth
		String tags = "";
		tags += stockInfo.exchange + " " + stockInfo.ticker + ",";
		tags += stockInfo.ticker + " " + stockInfo.exchange + ",";
		tags += stockInfo.name + ",";
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
			List<StockInfo> stockInfoList = new List<StockInfo>();
			while (reader.Read())
			{
				StockInfo stockInfo = new StockInfo();
				stockInfo.country = reader["country"].ToString();
				stockInfo.exchange = reader["exchange"].ToString();
				stockInfo.industry = reader["industry"].ToString();
				stockInfo.name = reader["company_name"].ToString();
				stockInfo.sector = reader["sector"].ToString();
				stockInfo.ticker = reader["ticker"].ToString();
				stockInfo.exchange = reader["exchange"].ToString();
				stockInfo.website = reader["website"].ToString();
				stockInfoList.Add(stockInfo);
			}
			reader.Close();
			foreach (StockInfo stockInfo in stockInfoList)
			{
				SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
				updateCommand.Parameters.AddWithValue("@tags", generate(stockInfo));
				updateCommand.Parameters.AddWithValue("@ticker", stockInfo.ticker);
				updateCommand.Parameters.AddWithValue("@exchange", stockInfo.exchange);
				updateCommand.ExecuteNonQuery();
			}
		}
	}
}