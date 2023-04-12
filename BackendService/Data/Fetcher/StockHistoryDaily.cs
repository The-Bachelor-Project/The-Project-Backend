using System.Data.SqlClient;
using Data.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data.Fetcher;

public class StockHistoryDaily : IStockHistoryDaily
{
	public StockHistoryDaily()
	{
	}

	public async Task<StockHistory> Usd(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
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
				StockHistory FromYahoo = await (new Data.YahooFinance.StockHistoryDaily()).Usd(ticker, exchange, startDate.AddDays(-7), endDate);
				_SaveStockHistory(FromYahoo, true, true);
				return FromYahoo;
			}

			reader.Close();

			if (startDate < StartTrackingDate)
			{
				StockHistory FromYahooBefore = await (new Data.YahooFinance.StockHistoryDaily()).Usd(ticker, exchange, startDate.AddDays(-7), StartTrackingDate.AddDays(-1));
				_SaveStockHistory(FromYahooBefore, true, false);
			}
			if (endDate > EndTrackingDate)
			{
				StockHistory FromYahooAfter = await (new Data.YahooFinance.StockHistoryDaily()).Usd(ticker, exchange, EndTrackingDate.AddDays(1), endDate);
				_SaveStockHistory(FromYahooAfter, false, true);
			}
		}

		return await (new Data.Database.StockHistoryDaily()).Usd(ticker, exchange, startDate, endDate);
	}



	void _SaveStockHistory(StockHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
		System.Console.WriteLine(history.History.Length);
		if (history.History.Length == 0)
			return;

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
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.First().Date));
			command.ExecuteNonQuery();
		}
		if (updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Stocks SET end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			command = new SqlCommand(updateEndTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.Ticker);
			command.Parameters.AddWithValue("@exchange", history.Exchange);
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.Last().Date));
			command.ExecuteNonQuery();
		}
	}
}