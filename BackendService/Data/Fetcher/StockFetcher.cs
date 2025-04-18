using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher;

public class StockFetcher : IStockFetcher
{
	/// <summary>
	/// Retrieves the historical price and dividend data for a specific stock in a specified date range.
	/// </summary>
	/// <param name="ticker">The stock ticker symbol.</param>
	/// <param name="exchange">The stock exchange.</param>
	/// <param name="startDate">The start date of the historical data.</param>
	/// <param name="endDate">The end date of the historical data.</param>
	/// <param name="interval">The interval of the historical data.</param>
	/// <param name="currency">The currency in which the data should be returned.</param>
	/// <returns>The historical price and dividend data for the specified stock.</returns>
	public async Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval, string currency)
	{
		if (ticker == null || exchange == null || interval == null || currency == null)
		{
			throw new StatusCodeException(400, "Required fields are missing");
		}
		if (!(Tools.ValidCurrency.Check(currency)))
		{
			throw new StatusCodeException(400, "Invalid currency: " + currency);
		}
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
				StockHistory fromYahoo = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), endDate, interval, currency);
				fromYahoo.dividends = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetDividends(ticker, exchange, startDate.AddDays(-7), endDate);
				Dictionary<DateOnly, Dictionary<int, int>> splitRatios = await new Data.Fetcher.YahooFinanceFetcher.SplitFetcher().GetSplits(ticker, exchange, startDate.AddDays(-7), endDate);
				SaveStockHistory(fromYahoo, true, true);
				SaveDividends(fromYahoo.dividends, ticker, exchange);
				SaveSplits(splitRatios, ticker, exchange);
				return fromYahoo;
			}
			if (startDate < startTrackingDate)
			{
				StockHistory fromYahooBefore = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, startDate.AddDays(-7), startTrackingDate.AddDays(-1), interval, currency);
				fromYahooBefore.dividends = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetDividends(ticker, exchange, startDate.AddDays(-7), startTrackingDate.AddDays(-1));
				Dictionary<DateOnly, Dictionary<int, int>> splitRatios = await new Data.Fetcher.YahooFinanceFetcher.SplitFetcher().GetSplits(ticker, exchange, startDate.AddDays(-7), startTrackingDate.AddDays(-1));
				SaveStockHistory(fromYahooBefore, true, false);
				SaveDividends(fromYahooBefore.dividends, ticker, exchange);
				SaveSplits(splitRatios, ticker, exchange);
			}
			if (endDate > endTrackingDate)
			{
				StockHistory fromYahooAfter = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetHistory(ticker, exchange, endTrackingDate.AddDays(1), endDate, interval, currency);
				fromYahooAfter.dividends = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetDividends(ticker, exchange, endTrackingDate.AddDays(1), endDate);
				Dictionary<DateOnly, Dictionary<int, int>> splitRatios = await new Data.Fetcher.YahooFinanceFetcher.SplitFetcher().GetSplits(ticker, exchange, endTrackingDate.AddDays(1), endDate);
				SaveStockHistory(fromYahooAfter, false, true);
				SaveDividends(fromYahooAfter.dividends, ticker, exchange);
				SaveSplits(splitRatios, ticker, exchange);
			}
		}
		StockHistory stockHistoryReturn = await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetHistory(ticker, exchange, startDate, endDate, interval, currency);
		stockHistoryReturn.history = await new Tools.PriceConverter().ConvertStockPrice(stockHistoryReturn.history, currency, true);
		stockHistoryReturn.dividends = await new Data.Fetcher.DatabaseFetcher.StockFetcher().GetDividends(ticker, exchange, startDate, endDate);
		stockHistoryReturn.dividends = await new Tools.PriceConverter().ConvertStockDividends(stockHistoryReturn.dividends, currency, true);
		return stockHistoryReturn;
	}

	/// <summary>
	/// Retrieves the profile information for a specific stock.
	/// </summary>
	/// <param name="ticker">The stock ticker symbol.</param>
	/// <param name="exchange">The stock exchange.</param>
	/// <returns>The profile information for the specified stock.</returns>
	public async Task<Data.StockProfile> GetProfile(string ticker, string exchange)
	{
		if (ticker == null || exchange == null)
		{
			throw new StatusCodeException(400, "Required fields are missing");
		}
		try
		{
			// Get the stock profile from the database
			DatabaseFetcher.StockFetcher database = new DatabaseFetcher.StockFetcher();
			Data.StockProfile profile = await database.GetProfile(ticker, exchange);
			return profile;
		}
		catch (StatusCodeException)
		{
			// If the stock profile is not in the database, get it from the API
			YahooFinanceFetcher.StockFetcher api = new YahooFinanceFetcher.StockFetcher();
			Data.StockProfile profile = await api.GetProfile(ticker, exchange);
			SaveProfile(profile);
			return profile;
		}
	}

	public async Task<Data.StockProfile[]> Search(string query)
	{
		if (query == null || query == "")
		{
			throw new StatusCodeException(400, "Required fields are missing");
		}
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
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Could not save stock profile of stock " + profile.exchange + ":" + profile.ticker + " to database");
		}


	}

	public String GenerateTags(Data.StockProfile stockProfile)
	{
		String tags = "";
		tags += stockProfile.exchange + " " + stockProfile.ticker + ",";
		tags += stockProfile.ticker + " " + stockProfile.exchange + ",";
		tags += stockProfile.displayName + ",";
		return tags.ToLower();
	}


	private void SaveStockHistory(StockHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
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
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.DateOnlyToString(history.history.First().date));
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
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.DateOnlyToString(history.history.Last().date));
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

	private void SaveSplits(Dictionary<DateOnly, Dictionary<int, int>> splitRatios, string ticker, string exchange)
	{
		foreach (KeyValuePair<DateOnly, Dictionary<int, int>> splitRatio in splitRatios)
		{
			foreach (KeyValuePair<int, int> ratio in splitRatio.Value)
			{
				String checkSplitExists = "SELECT COUNT(*) FROM StockSplits WHERE ticker = @ticker AND exchange = @exchange AND date = @date AND ratio_in = @ratio_in AND ratio_out = @ratio_out";
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@ticker", ticker);
				parameters.Add("@exchange", exchange);
				parameters.Add("@date", Tools.TimeConverter.DateOnlyToString(splitRatio.Key));
				parameters.Add("@ratio_in", ratio.Key);
				parameters.Add("@ratio_out", ratio.Value);
				Dictionary<string, object>? result = Data.Database.Reader.ReadOne(checkSplitExists, parameters);
				if (result != null && result.Count > 0)
					continue;

				String insertSplitRatioQuery = "INSERT INTO StockSplits (ticker, exchange, date, ratio_in, ratio_out) VALUES (@ticker, @exchange, @date, @ratio_in, @ratio_out)";
				SqlConnection connection = Data.Database.Connection.GetSqlConnection();
				SqlCommand command = new SqlCommand(insertSplitRatioQuery, connection);
				command.Parameters.AddWithValue("@ticker", ticker);
				command.Parameters.AddWithValue("@exchange", exchange);
				command.Parameters.AddWithValue("@date", Tools.TimeConverter.DateOnlyToString(splitRatio.Key));
				command.Parameters.AddWithValue("@ratio_out", ratio.Key);
				command.Parameters.AddWithValue("@ratio_in", ratio.Value);
				try
				{
					command.ExecuteNonQuery();
				}
				catch (SqlException e)
				{
					if (e.Number == 2627 || e.Number == 2601) // Primary key violations, meaning it exists
					{
						System.Console.WriteLine(e);
						continue;
					}
					else
					{
						System.Console.WriteLine(e);
						throw new StatusCodeException(500, "Error while saving split ratios");
					}

				}
				catch (Exception e)
				{
					System.Console.WriteLine(e);
					throw new StatusCodeException(500, "Error while saving split ratios");
				}
			}
		}
	}
}