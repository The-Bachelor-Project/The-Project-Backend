using System.Data.SqlClient;
using DatabaseService;

class CurrencyConversion
{
	public static async Task GetMissingRatesAsync(String ticker, String exchange, DateOnly StartDate, DateOnly EndDate)
	{
		String GetInfoQuery = "SELECT " +
									"Currencies.start_tracking_date AS currency_start_tracking," +
									"Currencies.end_tracking_date AS currency_end_tracking," +
									"Stocks.start_tracking_date AS stock_start_tracking," +
									"Stocks.end_tracking_date As stock_end_tracking," +
									"Exchanges.currency AS currency " +
									"FROM Stocks JOIN Exchanges ON Stocks.exchange = Exchanges.symbol LEFT JOIN Currencies ON Exchanges.currency = Currencies.code " +
									"WHERE Stocks.ticker = @ticker AND Stocks.exchange = @exchange";
		System.Console.WriteLine(GetInfoQuery);
		SqlConnection Connection = Database.createConnection();
		SqlCommand Command = new SqlCommand(GetInfoQuery, Connection);
		Command.Parameters.AddWithValue("@ticker", ticker);
		Command.Parameters.AddWithValue("@exchange", exchange);
		SqlDataReader Reader = Command.ExecuteReader();
		if (Reader.Read())
		{
			//TODO: Test how null from query is represented in C#
			System.Console.WriteLine(Reader.GetValue(0));
			if (Reader["currency_start_tracking"] == null)
			{
				//TODO: Handle if exchange is missing currency
				await CurrencyRatesUpdater.Update(Reader["currency"].ToString(), DateOnly.FromDateTime((DateTime)Reader["stock_start_tracking"]));
			}
			else
			{
				DateOnly CurrencyStartTrackingDate = DateOnly.FromDateTime((DateTime)Reader["currency_start_tracking"]);
				DateOnly CurrencyEndTrackingDate = DateOnly.FromDateTime((DateTime)Reader["currency_end_tracking"]);
				DateOnly StockStartTrackingDate = DateOnly.FromDateTime((DateTime)Reader["stock_start_tracking"]);
				DateOnly StockEndTrackingDate = DateOnly.FromDateTime((DateTime)Reader["stock_end_tracking"]);

				if (CurrencyStartTrackingDate < StartDate)
				{
					//TODO: Add so that it only get from stock start tracking date to currency start tracking date
					CurrencyRatesUpdater.Update(Reader["currency"].ToString(), DateOnly.FromDateTime((DateTime)Reader["stock_start_tracking"]));
				}
				else if (CurrencyEndTrackingDate > EndDate)
				{
					CurrencyRatesUpdater.Update(Reader["currency"].ToString(), DateOnly.FromDateTime((DateTime)Reader["currency_end_tracking"]));
				}
			}
		}
	}


}