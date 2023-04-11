using System.Data.SqlClient;
using Data.Interfaces;

namespace Data.Fetcher;

public class StockHistoryDaily : IStockHistoryDaily
{
	public StockHistoryDaily()
	{
	}

	public async Task<StockHistory> usd(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
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
				StockHistory FromYahoo = await (new Data.YahooFinance.StockHistoryDaily()).usd(ticker,exchange, startDate, endDate);
				SaveStockHistory(FromYahoo, true, true);
				return FromYahoo;
			}

			reader.Close();

			if (startDate < StartTrackingDate)
			{
				StockHistory FromYahooBefore = await (new Data.YahooFinance.StockHistoryDaily()).usd(ticker,exchange, startDate, StartTrackingDate.AddDays(-1));
				SaveStockHistory(FromYahooBefore, true, false);
			}
			if (endDate > EndTrackingDate)
			{
				StockHistory FromYahooAfter = await (new Data.YahooFinance.StockHistoryDaily()).usd(ticker, exchange, EndTrackingDate.AddDays(1), endDate);
				SaveStockHistory(FromYahooAfter, false, true);
			}
		}
		
		return await (new Data.Database.StockHistoryDaily()).usd(ticker, exchange, startDate, endDate);
	}



	void SaveStockHistory(StockHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate){
		if(history.History.Length == 0)
			return;

		String insertIntoStockPricesQuery = "INSERT INTO StockPrices VALUES (@ticker, @exchange, @date, @open_price, @high_price, @low_price, @close_price, @volume)";
		//TODO Use something like INSERT OR UPDATE instead of just INSERT (Maybe MERGE)
		//TODO Also look into making a DB function that takes a json array to speed up the process

		SqlConnection connection = DatabaseService.Database.createConnection();
		foreach(StockHistoryData data in history.History)
		{


			SqlCommand command = new SqlCommand(insertIntoStockPricesQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.Ticker);
			command.Parameters.AddWithValue("@exchange", history.Exchange);
			command.Parameters.AddWithValue("@date", Tools.TimeConverter.dateOnlyToString(data.Date));
			command.Parameters.AddWithValue("@open_price", data.OpenPrice);
			command.Parameters.AddWithValue("@high_price", data.HighPrice);
			command.Parameters.AddWithValue("@low_price", data.LowPrice);
			command.Parameters.AddWithValue("@close_price", data.ClosePrice);
			command.Parameters.AddWithValue("@volume", 0);
			command.ExecuteNonQuery();
		}
		if(updateStartTrackingDate)
		{
			String updateStartTrackingDateQuery = "UPDATE Stocks SET start_tracking_date = @start_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			connection = DatabaseService.Database.createConnection();
			SqlCommand command = new SqlCommand(updateStartTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.Ticker);
			command.Parameters.AddWithValue("@exchange", history.Exchange);
			command.Parameters.AddWithValue("@start_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.First().Date));
			command.ExecuteNonQuery();
		}
		if(updateEndTrackingDate)
		{
			String updateEndTrackingDateQuery = "UPDATE Stocks SET end_tracking_date = @end_tracking_date WHERE ticker = @ticker AND exchange = @exchange";
			connection = DatabaseService.Database.createConnection();
			SqlCommand command = new SqlCommand(updateEndTrackingDateQuery, connection);
			command.Parameters.AddWithValue("@ticker", history.Ticker);
			command.Parameters.AddWithValue("@exchange", history.Exchange);
			command.Parameters.AddWithValue("@end_tracking_date", Tools.TimeConverter.dateOnlyToString(history.History.Last().Date));
			command.ExecuteNonQuery();
		}
	}
}