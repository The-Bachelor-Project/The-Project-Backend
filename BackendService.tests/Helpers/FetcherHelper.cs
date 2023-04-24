using System.Data.SqlClient;
using Data;

public class FetcherHelper
{
	public static Boolean ResetHistory(String ticker, String exchange, int isStock)
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "EXEC ResetIt @ticker, @exchange, @isStock";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		command.Parameters.AddWithValue("@isStock", isStock);
		try
		{
			command.ExecuteNonQuery();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static Boolean DeleteStockProfile(String ticker, String exchange)
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "DELETE FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@ticker", ticker);
		command.Parameters.AddWithValue("@exchange", exchange);
		try
		{
			command.ExecuteNonQuery();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static async Task<Boolean> SaveStockHistoryToDB(String ticker, String exchange)
	{
		StockHistory result = await new Data.Fetcher.StockFetcher().GetHistory(ticker, exchange, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"), "daily");
		return result != null && result.history.Count > 0;
	}

	public static async Task<Boolean> SaveStockProfileToDB(String ticker, String exchange)
	{
		StockProfile result = await new Data.Fetcher.StockFetcher().GetProfile(ticker, exchange);
		return result != null;
	}

	public static async Task<Boolean> SaveCurrencyHistoryToDB(String code)
	{
		CurrencyHistory result = await new Data.Fetcher.CurrencyFetcher().GetHistory(code, DateOnly.Parse("2021-01-01"), DateOnly.Parse("2022-01-01"));
		return result != null && result.history.Count > 0;
	}
}