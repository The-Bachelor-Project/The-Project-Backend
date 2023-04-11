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
		try{
			StockHistory Result = await (new Data.Database.StockHistoryDaily()).usd(ticker, exchange, startDate, endDate);
			if(Result.StartDate!.Value.AddDays(-1) > startDate)
			{
				StockHistory FromYahooBefore = await (new Data.YahooFinance.StockHistoryDaily()).usd(ticker,exchange, startDate, Result.StartDate.Value.AddDays(-1));
				//SaveStockHistory(FromYahooBefore);
				Result.History = FromYahooBefore.History.Concat(Result.History).ToArray();
				Result.StartDate = FromYahooBefore.StartDate;
			}
			if(Result.EndDate < endDate)
			{
				StockHistory FromYahooAfter = await (new Data.YahooFinance.StockHistoryDaily()).usd(ticker, exchange, (Result.EndDate.Value).AddDays(1), endDate);
				//SaveStockHistory(FromYahooAfter);
				Result.History = Result.History.Concat(FromYahooAfter.History).ToArray();
				Result.EndDate = FromYahooAfter.EndDate;
			}

			return Result;
		}
		catch(Exception){
			StockHistory Result = await (new Data.YahooFinance.StockHistoryDaily()).usd(ticker, exchange, startDate, endDate);

			return Result;
		}
	}

	void SaveStockHistory(StockHistory history){
		String insertIntoStockPricesQuery = "INSERT INTO StockPrices VALUES (@ticker, @exchange, @date, @open_price, @high_price, @low_price, @close_price, @volume)";
		foreach(StockHistoryData data in history.History)
		{
			SqlConnection connection = DatabaseService.Database.createConnection();
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
	}
}