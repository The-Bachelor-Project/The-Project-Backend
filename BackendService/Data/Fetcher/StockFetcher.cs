using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher;

public class StockFetcher : IStockFetcher
{
	public async Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval)
	{
		SqlConnection connection = (new Database.Connection()).Create();
		String getTrackingDateQuery = "SELECT start_tracking_date, end_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		SqlCommand command = new SqlCommand(getTrackingDateQuery, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		SqlDataReader reader = command.ExecuteReader();

		if (reader.Read())
		{
			DateOnly StartTrackingDate;
			DateOnly EndTrackingDate;
			try
			{
				StartTrackingDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]);
				EndTrackingDate = DateOnly.FromDateTime((DateTime)reader["end_tracking_date"]);
			}
			catch (Exception)
			{
				StockHistory FromYahoo = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), endDate, interval);
				SaveStockHistory(FromYahoo, true, true);
				return FromYahoo;
			}

			reader.Close();

			if (startDate < StartTrackingDate)
			{
				StockHistory FromYahooBefore = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), StartTrackingDate.AddDays(-1), interval);
				SaveStockHistory(FromYahooBefore, true, false);
			}
			if (endDate > EndTrackingDate)
			{
				StockHistory FromYahooAfter = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, EndTrackingDate.AddDays(1), endDate, interval);
				SaveStockHistory(FromYahooAfter, false, true);
			}
		}

		return await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetHistory(ticker, exchange, startDate, endDate, interval);
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
			SaveProfile(Profile);
			return Profile;
		}
	}

	public async Task<Data.StockProfile[]> Search(string query)
	{
		await new YahooFinanceFetcher.StockFetcher().Search(query); //TODO in the future check if search has already been performed recently
		return await new DatabaseFetcher.StockFetcher().Search(query);
	}



	private void SaveProfile(Data.StockProfile profile)
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


	private void SaveStockHistory(StockHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
		System.Console.WriteLine(history.History.Count);
		if (history.History.Count == 0)
			return;
		//TODO FIXME StockPricesBulk is now broken
		String InsertIntoStockPricesQuery = "EXEC BulkJsonStockPrices @StockPricesBulk, @Ticker, @Exchange";
		dynamic JsonStockPrices = JsonConvert.SerializeObject(history.History);
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand();

		command = new SqlCommand(InsertIntoStockPricesQuery, connection);
		command.Parameters.AddWithValue("@StockPricesBulk", JsonStockPrices);
		command.Parameters.AddWithValue("@Ticker", history.Ticker);
		command.Parameters.AddWithValue("@Exchange", history.Exchange);
		command.ExecuteNonQuery();


		if (updateStartTrackingDate)
		{
			String updateStartTrackingDateQuery = "UPDATE Stocks SET start_tracking_date = @start_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.Ticker);
			command.Parameters.AddWithValue("@exchange", history.Exchange);
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.First().date));
			command.ExecuteNonQuery();
		}
		if (updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Stocks SET end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateEndTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.Ticker);
			command.Parameters.AddWithValue("@exchange", history.Exchange);
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.Last().date));
			command.ExecuteNonQuery();
		}
	}
}