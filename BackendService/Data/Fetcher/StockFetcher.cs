using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher;

public class StockFetcher : IStockFetcher
{
	public async Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval)
	{
		if (startDate > endDate)
		{
			throw new StatusCodeException(400, "Start date cannot be after end date");
		}
		String getTrackingDateQuery = "SELECT start_tracking_date, end_tracking_date FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getTrackingDateQuery, parameters);

		if (data != null)
		{
			DateOnly startTrackingDate;
			DateOnly endTrackingDate;
			try
			{
				startTrackingDate = DateOnly.FromDateTime((DateTime)data["start_tracking_date"]);
				endTrackingDate = DateOnly.FromDateTime((DateTime)data["end_tracking_date"]);
			}
			catch (Exception)
			{
				StockHistory fromYahoo = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), endDate, interval);
				fromYahoo.dividends = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetDividends(ticker, exchange, startDate.AddDays(-7), endDate);
				SaveStockHistory(fromYahoo, true, true);
				SaveDividends(fromYahoo.dividends, ticker, exchange);
				return fromYahoo;
			}
			if (startDate < startTrackingDate)
			{
				StockHistory fromYahooBefore = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), startTrackingDate.AddDays(-1), interval);
				fromYahooBefore.dividends = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetDividends(ticker, exchange, startDate.AddDays(-7), startTrackingDate.AddDays(-1));
				SaveStockHistory(fromYahooBefore, true, false);
				SaveDividends(fromYahooBefore.dividends, ticker, exchange);
			}
			if (endDate > endTrackingDate)
			{
				StockHistory fromYahooAfter = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, endTrackingDate.AddDays(1), endDate, interval);
				fromYahooAfter.dividends = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetDividends(ticker, exchange, endTrackingDate.AddDays(1), endDate);
				SaveStockHistory(fromYahooAfter, false, true);
				SaveDividends(fromYahooAfter.dividends, ticker, exchange);
			}
		}
		StockHistory stockHistoryReturn = await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetHistory(ticker, exchange, startDate, endDate, interval);
		stockHistoryReturn.dividends = await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetDividends(ticker, exchange, startDate, endDate);
		return stockHistoryReturn;
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
		catch (StatusCodeException)
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
		//await new YahooFinanceFetcher.StockFetcher().Search(query); //TODO in the future check if search has already been performed recently
		return await new DatabaseFetcher.StockFetcher().Search(query);
	}



	private void SaveProfile(Data.StockProfile profile)
	{
		String tags = GenerateTags(profile);
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String query = "INSERT INTO Stocks (ticker, exchange, company_name, short_name, long_name, address, city, state, zip, financial_currency, shares_outstanding, industry, sector, website, country, trailing_annual_dividend_rate, trailing_annual_dividend_yield, tags) VALUES (@ticker, @exchange, @name, @short_name, @long_name, @address, @city, @state, @zip, @financial_currency, @shares_outstanding, @industry, @sector, @website, @country, @trailing_annual_dividend_rate, @trailing_annual_dividend_yield, @tags)";
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
		command.Parameters.AddWithValue("@trailing_annual_dividend_rate", profile.trailingAnnualDividendRate);
		command.Parameters.AddWithValue("@trailing_annual_dividend_yield", profile.trailingAnnualDividendYield);
		command.Parameters.AddWithValue("@tags", tags);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e.Message);
			throw new StatusCodeException(500, "Could not save stock profile of stock " + profile.exchange + ":" + profile.ticker + " to database");
		}


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
		String insertIntoStockPricesQuery = "EXEC BulkJsonStockPrices @StockPricesBulk, @Ticker, @Exchange";
		dynamic jsonStockPrices = JsonConvert.SerializeObject(history.history);
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand();

		command = new SqlCommand(insertIntoStockPricesQuery, connection);
		command.Parameters.AddWithValue("@StockPricesBulk", jsonStockPrices);
		command.Parameters.AddWithValue("@Ticker", history.ticker);
		command.Parameters.AddWithValue("@Exchange", history.exchange);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e.Message);
			throw new StatusCodeException(500, "Could not save stock history of " + history.exchange + ":" + history.ticker + " to database");
		}


		if (updateStartTrackingDate)
		{
			String updateStartTrackingDateQuery = "UPDATE Stocks SET start_tracking_date = @start_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.ticker);
			command.Parameters.AddWithValue("@exchange", history.exchange);
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.First().date));
			try
			{
				command.ExecuteNonQuery();
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.Message);
				throw new StatusCodeException(500, "Could not update start tracking date of " + history.exchange + ":" + history.ticker + " in database");
			}
		}
		if (updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Stocks SET end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateEndTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.ticker);
			command.Parameters.AddWithValue("@exchange", history.exchange);
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.history.Last().date));
			try
			{
				command.ExecuteNonQuery();
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e.Message);
				throw new StatusCodeException(500, "Could not update end tracking date of " + history.exchange + ":" + history.ticker + " in database");

			}
		}
	}

	private void SaveDividends(List<Data.Dividend> dividends, String ticker, String exchange)
	{
		if (dividends.Count == 0)
			return;
		String insertIntoDividendsQuery = "EXEC BulkJsonStockDividends @StockDividendsBulk, @Ticker, @Exchange";
		dynamic jsonDividends = JsonConvert.SerializeObject(dividends);
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand();

		command = new SqlCommand(insertIntoDividendsQuery, connection);
		command.Parameters.AddWithValue("@StockDividendsBulk", jsonDividends);
		command.Parameters.AddWithValue("@Ticker", ticker);
		command.Parameters.AddWithValue("@Exchange", exchange);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e.Message);
			throw new StatusCodeException(500, "Could not save dividends of " + exchange + ":" + ticker + " to database");
		}
	}
}