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
		using (SqlDataReader reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				DateOnly startTrackingDate;
				DateOnly endTrackingDate;
				try
				{
					startTrackingDate = DateOnly.FromDateTime((DateTime)reader["start_tracking_date"]);
					endTrackingDate = DateOnly.FromDateTime((DateTime)reader["end_tracking_date"]);
					reader.Close();
				}
				catch (Exception)
				{
					reader.Close();
					StockHistory fromYahoo = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), endDate, interval);
					SaveStockHistory(fromYahoo, true, true);
					return fromYahoo;
				}
				if (startDate < startTrackingDate)
				{
					StockHistory fromYahooBefore = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), startTrackingDate.AddDays(-1), interval);
					SaveStockHistory(fromYahooBefore, true, false);
				}
				if (endDate > endTrackingDate)
				{
					StockHistory fromYahooAfter = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, endTrackingDate.AddDays(1), endDate, interval);
					SaveStockHistory(fromYahooAfter, false, true);
				}
			}
			reader.Close();
			return await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetHistory(ticker, exchange, startDate, endDate, interval);
		}
	}

	public async Task<Data.StockProfile> GetProfile(string ticker, string exchange)
	{
		try
		{
			// Get the stock profile from the database
			DatabaseFetcher.StockFetcher database = new DatabaseFetcher.StockFetcher();
			Data.StockProfile profile = await database.GetProfile(ticker, exchange);
			System.Console.WriteLine("Got stock profile from database");
			return profile;
		}
		catch (CouldNotGetStockException)
		{
			// If the stock profile is not in the database, get it from the API
			YahooFinanceFetcher.StockFetcher api = new YahooFinanceFetcher.StockFetcher();
			Data.StockProfile profile = await api.GetProfile(ticker, exchange);
			System.Console.WriteLine("Got stock profile from API");
			SaveProfile(profile);
			return profile;
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
		String query = "INSERT INTO Stocks (ticker, exchange, company_name, short_name, long_name, address, city, state, zip, financial_currency, shares_outstanding, industry, sector, website, country, tags) VALUES (@ticker, @exchange, @name, @short_name, @long_name, @address, @city, @state, @zip, @financial_currency, @shares_outstanding, @industry, @sector, @website, @country, @tags)";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@ticker", profile.ticker);
		command.Parameters.AddWithValue("@exchange", profile.exchange);
		command.Parameters.AddWithValue("@name", profile.displayName);
		command.Parameters.AddWithValue("@short_name", profile.shortName);
		command.Parameters.AddWithValue("@long_name", profile.longName);
		command.Parameters.AddWithValue("@address", profile.address);
		command.Parameters.AddWithValue("@city", profile.city);
		command.Parameters.AddWithValue("@state", profile.state);
		command.Parameters.AddWithValue("@zip", profile.zip);
		command.Parameters.AddWithValue("@financial_currency", profile.financialCurrency);
		command.Parameters.AddWithValue("@shares_outstanding", profile.sharesOutstanding);
		command.Parameters.AddWithValue("@industry", profile.industry);
		command.Parameters.AddWithValue("@sector", profile.sector);
		command.Parameters.AddWithValue("@website", profile.website);
		command.Parameters.AddWithValue("@country", profile.country);
		command.Parameters.AddWithValue("@tags", tags);
		command.ExecuteNonQuery();
	}

	public String GenerateTags(Data.StockProfile stockProfile)
	{
		//TODO: Make variants for NOVO-B, NOVO B, AT&T, ATT, AT T, so fourth
		String tags = "";
		tags += stockProfile.exchange + " " + stockProfile.ticker + ",";
		tags += stockProfile.ticker + " " + stockProfile.exchange + ",";
		tags += stockProfile.displayName + ",";
		return tags.ToLower();
	}


	private void SaveStockHistory(StockHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
		System.Console.WriteLine(history.history.Count);
		if (history.history.Count == 0)
			return;
		//TODO FIXME StockPricesBulk is now broken
		String insertIntoStockPricesQuery = "EXEC BulkJsonStockPrices @StockPricesBulk, @Ticker, @Exchange";
		dynamic jsonStockPrices = JsonConvert.SerializeObject(history.history);
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand();

		command = new SqlCommand(insertIntoStockPricesQuery, connection);
		command.Parameters.AddWithValue("@StockPricesBulk", jsonStockPrices);
		command.Parameters.AddWithValue("@Ticker", history.ticker);
		command.Parameters.AddWithValue("@Exchange", history.exchange);
		command.ExecuteNonQuery();


		if (updateStartTrackingDate)
		{
			String updateStartTrackingDateQuery = "UPDATE Stocks SET start_tracking_date = @start_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.ticker);
			command.Parameters.AddWithValue("@exchange", history.exchange);
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.First().date));
			command.ExecuteNonQuery();
		}
		if (updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Stocks SET end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateEndTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.ticker);
			command.Parameters.AddWithValue("@exchange", history.exchange);
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.Last().date));
			command.ExecuteNonQuery();
		}
	}
}