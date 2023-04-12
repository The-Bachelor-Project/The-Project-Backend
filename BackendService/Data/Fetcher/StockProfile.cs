using System.Data.SqlClient;
using Data.Database;
using Data.Interfaces;

namespace Data.Fetcher;

class StockProfile : IStockProfile
{
	public async Task<Data.StockProfile> Get(string ticker, string exchange)
	{
		try
		{
			// Get the stock profile from the database
			Data.Database.StockProfile Database = new Data.Database.StockProfile();
			Data.StockProfile Profile = await Database.Get(ticker, exchange);
			System.Console.WriteLine("Got stock profile from database");
			return Profile;
		}
		catch (CouldNotGetStockException)
		{
			// If the stock profile is not in the database, get it from the API
			Data.YahooFinance.StockProfile API = new Data.YahooFinance.StockProfile();
			Data.StockProfile Profile = await API.Get(ticker, exchange);
			System.Console.WriteLine("Got stock profile from API");
			_Save(Profile);
			return Profile;
		}
	}

	private void _Save(Data.StockProfile profile)
	{
		String tags = _GenerateTags(profile);
		SqlConnection connection = new Connection().Create();
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

	public String _GenerateTags(Data.StockProfile stockProfile)
	{
		//TODO: Make variants for NOVO-B, NOVO B, AT&T, ATT, AT T, so fourth
		String tags = "";
		tags += stockProfile.Exchange + " " + stockProfile.Ticker + ",";
		tags += stockProfile.Ticker + " " + stockProfile.Exchange + ",";
		tags += stockProfile.Name + ",";
		return tags.ToLower();
	}
}