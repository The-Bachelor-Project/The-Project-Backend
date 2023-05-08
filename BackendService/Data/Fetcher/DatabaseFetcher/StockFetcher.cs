using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Data.Fetcher.Interfaces;

namespace Data.Fetcher.DatabaseFetcher;

public class StockFetcher : IStockFetcher
{
	public Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval)
	{
		System.Console.WriteLine("Getting stock history from database for " + ticker + " " + exchange + " " + startDate + " " + endDate + " " + interval);
		StockHistory result = new StockHistory(ticker, exchange, "daily");

		SqlConnection connection = new Data.Database.Connection().Create();
		String getStockHistoryQuery = "SELECT * FROM GetStockPrices(@ticker, @exchange, 'daily', @start_date, @end_date)";
		SqlCommand command = new SqlCommand(getStockHistoryQuery, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		command.Parameters.AddWithValue("@start_date", Tools.TimeConverter.dateOnlyToString(startDate));
		command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
		using (SqlDataReader reader = command.ExecuteReader())
		{
			while (reader.Read())
			{
				result.history.Add(new Data.DatePriceOHLC(
					DateOnly.FromDateTime((DateTime)reader["end_date"]),
					new Money(Decimal.Parse("" + reader["open_price"].ToString()), Data.Money.DEFAULT_CURRENCY),
					new Money(Decimal.Parse("" + reader["high_price"].ToString()), Data.Money.DEFAULT_CURRENCY),
					new Money(Decimal.Parse("" + reader["low_price"].ToString()), Data.Money.DEFAULT_CURRENCY),
					new Money(Decimal.Parse("" + reader["close_price"].ToString()), Data.Money.DEFAULT_CURRENCY)
				));
			}
			result.startDate = result.history.First().date;
			result.endDate = result.history.Last().date;
			reader.Close();

			return Task.FromResult(result);
		}
	}

	public Task<Data.StockProfile> GetProfile(string ticker, string exchange)
	{
		Data.StockProfile profile = new Data.StockProfile();
		profile.ticker = ticker;
		profile.exchange = exchange;


		SqlConnection connection = (new Data.Database.Connection()).Create();
		String query = "SELECT * FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				profile.displayName = reader["company_name"].ToString();
				profile.shortName = reader["short_name"].ToString();
				profile.longName = reader["long_name"].ToString();
				profile.address = reader["address"].ToString();
				profile.city = reader["city"].ToString();
				profile.state = reader["state"].ToString();
				profile.zip = reader["zip"].ToString();
				profile.financialCurrency = reader["financial_currency"].ToString();
				profile.sharesOutstanding = (Decimal.TryParse(reader["shares_outstanding"].ToString(), out decimal number) ? number : 0);
				profile.industry = reader["industry"].ToString();
				profile.sector = reader["sector"].ToString();
				profile.website = reader["website"].ToString();
				profile.country = reader["country"].ToString();
			}
			else
			{
				reader.Close();
				throw new CouldNotGetStockException();
			}
			reader.Close();
			return Task.FromResult(profile);
		}


	}

	public Task<Data.StockProfile[]> Search(string query)
	{
		Data.StockProfile[] results = new Data.StockProfile[] { };
		Regex regex = new Regex("[A-Za-z0-9]*[A-Za-z0-9]", RegexOptions.IgnoreCase);
		MatchCollection matchedAuthors = regex.Matches(query);
		String termTrimmed = matchedAuthors[0].Value.ToLower();
		for (int i = 1; i < matchedAuthors.Count; i++)
		{
			termTrimmed += " " + matchedAuthors[i].Value.ToLower();
		}

		SqlConnection connection = new Data.Database.Connection().Create();
		String sqlQuery = "SELECT TOP 100 * FROM Stocks WHERE tags LIKE @tags";
		SqlCommand command = new SqlCommand(sqlQuery, connection);
		command.Parameters.AddWithValue("@tags", "%" + query + "%");
		using (SqlDataReader reader = command.ExecuteReader())
		{
			while (reader.Read())
			{
				results = results.Append(new Data.StockProfile((String)reader["ticker"], (String)reader["exchange"], (String)reader["company_name"], (String)reader["country"])).ToArray();
			}
			reader.Close();
			return Task.FromResult(results);
		}
	}

	public Task<List<Dividend>> GetDividends(string ticker, string exchange, DateOnly startDate, DateOnly endDate)
	{
		System.Console.WriteLine("Getting dividends from database for " + ticker + " " + exchange + " " + startDate + " " + endDate);
		List<Dividend> dividends = new List<Dividend>();
		SqlConnection connection = new Data.Database.Connection().Create();
		String getDividendsQuery = "SELECT * FROM StockDividends WHERE ticker = @ticker AND exchange = @exchange AND date >= @start_date AND date <= @end_date";
		SqlCommand command = new SqlCommand(getDividendsQuery, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		command.Parameters.AddWithValue("@start_date", Tools.TimeConverter.dateOnlyToString(startDate));
		command.Parameters.AddWithValue("@end_date", Tools.TimeConverter.dateOnlyToString(endDate));
		using (SqlDataReader reader = command.ExecuteReader())
		{
			while (reader.Read())
			{
				dividends.Add(new Data.Dividend(
					DateOnly.FromDateTime((DateTime)reader["date"]),
					new Money(Decimal.Parse("" + reader["payout"].ToString()), Data.Money.DEFAULT_CURRENCY)
				));
			}
			reader.Close();
			return Task.FromResult(dividends);
		}
	}
}