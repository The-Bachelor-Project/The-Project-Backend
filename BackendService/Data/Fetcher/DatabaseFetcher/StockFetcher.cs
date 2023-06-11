using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Data.Fetcher.Interfaces;

namespace Data.Fetcher.DatabaseFetcher;

public class StockFetcher : IStockFetcher
{
	public Task<StockHistory> GetHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, string interval, string currency)
	{
		if (startDate > endDate)
		{
			throw new StatusCodeException(400, "Start date must be before end date");
		}
		if (ticker == null || exchange == null || interval == null || currency == null)
		{
			throw new StatusCodeException(400, "Required fields missing");
		}
		if (!(Tools.ValidCurrency.Check(currency)))
		{
			throw new StatusCodeException(400, "Invalid currency");
		}
		StockHistory result = new StockHistory(ticker, exchange, "daily");

		String getStockHistoryQuery = "SELECT * FROM GetStockPrices(@ticker, @exchange, 'daily', @start_date, @end_date)";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		parameters.Add("@start_date", Tools.TimeConverter.DateOnlyToString(startDate));
		parameters.Add("@end_date", Tools.TimeConverter.DateOnlyToString(endDate));
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(getStockHistoryQuery, parameters);
		if (data.Count == 0)
		{
			throw new StatusCodeException(404, "No stock history data found for " + exchange + ":" + ticker + " from database. Please check if exchange and ticker are correct.");
		}

		foreach (Dictionary<String, object> row in data)
		{
			try
			{
				result.history.Add(new Data.DatePriceOHLC(
					DateOnly.FromDateTime((DateTime)row["end_date"]),
					new StockApp.Money(Decimal.Parse("" + row["open_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY),
					new StockApp.Money(Decimal.Parse("" + row["high_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY),
					new StockApp.Money(Decimal.Parse("" + row["low_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY),
					new StockApp.Money(Decimal.Parse("" + row["close_price"].ToString()), StockApp.Money.DEFAULT_CURRENCY)
				));
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e);
				continue;
			}
		}
		result.startDate = result.history.First().date;
		result.endDate = result.history.Last().date;
		return Task.FromResult(result);
	}

	public Task<Data.StockProfile> GetProfile(string ticker, string exchange)
	{
		Data.StockProfile profile = new Data.StockProfile();
		profile.ticker = ticker;
		profile.exchange = exchange;

		String query = "SELECT * FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);

		if (data != null)
		{
			profile.displayName = data["company_name"].ToString();
			profile.shortName = data["short_name"].ToString();
			profile.longName = data["long_name"].ToString();
			profile.address = data["address"].ToString();
			profile.city = data["city"].ToString();
			profile.state = data["state"].ToString();
			profile.zip = data["zip"].ToString();
			profile.financialCurrency = data["financial_currency"].ToString();
			profile.sharesOutstanding = (Decimal.TryParse(data["shares_outstanding"].ToString(), out decimal number) ? number : 0);
			profile.industry = data["industry"].ToString();
			profile.sector = data["sector"].ToString();
			profile.website = data["website"].ToString();
			profile.country = data["country"].ToString();

			return Task.FromResult(profile);
		}
		else
		{
			throw new StatusCodeException(0);
		}
	}

	public Task<Data.StockProfile[]> Search(string query)
	{
		if (query == null)
		{
			throw new StatusCodeException(400, "Required fields missing");
		}
		Data.StockProfile[] results = new Data.StockProfile[] { };
		Regex regex = new Regex("[A-Za-z0-9]*[A-Za-z0-9]", RegexOptions.IgnoreCase);
		MatchCollection matchedAuthors = regex.Matches(query);
		String termTrimmed = matchedAuthors[0].Value.ToLower();
		for (int i = 1; i < matchedAuthors.Count; i++)
		{
			termTrimmed += " " + matchedAuthors[i].Value.ToLower();
		}

		String sqlQuery = "SELECT TOP 100 * FROM Stocks WHERE tags LIKE @tags";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@tags", "%" + query + "%");
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(sqlQuery, parameters);

		foreach (Dictionary<String, object> row in data)
		{
			results = results.Append(new Data.StockProfile((String)row["ticker"], (String)row["exchange"], (String)row["company_name"], (String)row["short_name"], (String)row["long_name"], (String)row["country"])).ToArray();
		}
		return Task.FromResult(results);
	}

	public Task<List<Dividend>> GetDividends(string ticker, string exchange, DateOnly startDate, DateOnly endDate)
	{
		List<Dividend> dividends = new List<Dividend>();
		String getDividendsQuery = "SELECT * FROM StockDividends WHERE ticker = @ticker AND exchange = @exchange AND date >= @start_date AND date <= @end_date";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@ticker", ticker);
		parameters.Add("@exchange", exchange);
		parameters.Add("@start_date", Tools.TimeConverter.DateOnlyToString(startDate));
		parameters.Add("@end_date", Tools.TimeConverter.DateOnlyToString(endDate));
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(getDividendsQuery, parameters);
		foreach (Dictionary<String, object> row in data)
		{
			try
			{
				dividends.Add(new Dividend(
					DateOnly.FromDateTime((DateTime)row["date"]),
					new StockApp.Money(Decimal.Parse("" + row["payout"].ToString()), StockApp.Money.DEFAULT_CURRENCY)
				));
			}
			catch (System.Exception e)
			{
				System.Console.WriteLine(e);
				continue;
			}

		}
		return Task.FromResult(dividends);
	}
}