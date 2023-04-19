using System.Data.SqlClient;
using Data.Fetcher.Interfaces;

namespace Data.Fetcher;

public class StockFetcher : IStockFetcher
{
	public Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval)
	{
		throw new NotImplementedException();
	}

	public async Task<Data.StockProfile> GetProfile(string ticker, string exchange)
	{
		try
		{
			// Get the stock profile from the database
			DatabaseFetcher.StockFetcher Database = new DatabaseFetcher.StockFetcher();
			Data.StockProfile Profile = await Database.GetProfile(ticker, exchange);
			System.Console.WriteLine("Got stock profile from database");
			return Profile;
		}
		catch (CouldNotGetStockException)
		{
			// If the stock profile is not in the database, get it from the API
			YahooFinanceFetcher.StockFetcher API = new YahooFinanceFetcher.StockFetcher();
			Data.StockProfile Profile = await API.GetProfile(ticker, exchange);
			System.Console.WriteLine("Got stock profile from API");
			Save(Profile);
			return Profile;
		}
	}

	public async Task<Data.StockProfile[]> Search(string query)
	{
		await new YahooFinanceFetcher.StockFetcher().Search(query); //TODO in the future check if search has already been performed recently
		return await new DatabaseFetcher.StockFetcher().Search(query);
	}



	private void Save(Data.StockProfile profile)
	{
		String tags = GenerateTags(profile);
		SqlConnection connection = new Data.Database.Connection().Create();
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

	public String GenerateTags(Data.StockProfile stockProfile)
	{
		//TODO: Make variants for NOVO-B, NOVO B, AT&T, ATT, AT T, so fourth
		String tags = "";
		tags += stockProfile.Exchange + " " + stockProfile.Ticker + ",";
		tags += stockProfile.Ticker + " " + stockProfile.Exchange + ",";
		tags += stockProfile.Name + ",";
		return tags.ToLower();
	}
}