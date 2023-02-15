using System.Data.SqlClient;

namespace DatabaseService;

class StockProfile
{
	public static async Task<Data.StockProfile> Get(String ticker, String exchange)
	{
		Data.StockProfile profile = new Data.StockProfile();


		SqlConnection connection = Database.createConnection();
		String query = "SELECT * FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		SqlDataReader reader = command.ExecuteReader();
		try
		{
			if (reader.Read())
			{
				profile.Name = reader["company_name"].ToString();
				profile.Industry = reader["industry"].ToString();
				profile.Sector = reader["sector"].ToString();
				profile.Website = reader["website"].ToString();
				profile.Country = reader["country"].ToString();
			}
			else
			{
				profile = await BackendService.DataFetcher.stock(ticker, exchange);
				_Save(profile);
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new CouldNotGetStockException();
		}

		return profile;
	}

	private static void _Save(Data.StockProfile profile)
	{
		String tags = StockTagGenerator.generate(profile);
		SqlConnection connection = Database.createConnection();
		String query = "INSERT INTO Stocks (ticker, exchange, company_name, industry, sector, website, country, tags) VALUES (@ticker, @exchange, @name, @industry, @sector, @website, @country, @tags)";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@ticker", profile.Ticker);
		command.Parameters.AddWithValue("@exchange", profile.Exchange);
		command.Parameters.AddWithValue("@name", profile.Name);
		command.Parameters.AddWithValue("@industry", profile.Industry);
		command.Parameters.AddWithValue("@sector", profile.Sector);
		command.Parameters.AddWithValue("@website", profile.Website);
		command.Parameters.AddWithValue("@country", profile.Country);
		command.Parameters.AddWithValue("@tags", tags);
		command.ExecuteNonQuery();
	}
}