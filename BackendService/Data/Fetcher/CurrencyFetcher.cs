using System.Data.SqlClient;
using Data.Fetcher.Interfaces;
using Newtonsoft.Json;

namespace Data.Fetcher;

public class CurrencyFetcher : ICurrencyFetcher
{

	/// <summary>
	/// Retrieves the historical data for a specific currency within a date range.
	/// </summary>
	/// <param name="currency">The currency code.</param>
	/// <param name="startDate">The start date of the historical data.</param>
	/// <param name="endDate">The end date of the historical data.</param>
	/// <returns>The historical data for the specified currency.</returns>
	public async Task<Data.CurrencyHistory> GetHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		String getTrackingDateQuery = "SELECT start_tracking_date, end_tracking_date FROM Currencies WHERE code = @currency";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@currency", currency);
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
				Data.CurrencyHistory fromYahoo = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), endDate);
				SaveCurrencyHistory(fromYahoo, true, true);
				return InsertMissingValues(fromYahoo);
			}

			if (startDate < startTrackingDate)
			{
				Data.CurrencyHistory fromYahooBefore = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, startDate.AddDays(-7), startTrackingDate.AddDays(-1));

				SaveCurrencyHistory(fromYahooBefore, true, false);
			}
			if (endDate > endTrackingDate)
			{
				Data.CurrencyHistory fromYahooAfter = await (new Data.Fetcher.YahooFinanceFetcher.CurrencyFetcher()).GetHistory(currency, endTrackingDate.AddDays(1), endDate);
				SaveCurrencyHistory(fromYahooAfter, false, true);
			}
		}
		CurrencyHistory currencyHistory = await new Data.Fetcher.DatabaseFetcher.CurrencyFetcher().GetHistory(currency, startDate, endDate);
		return InsertMissingValues(currencyHistory);
	}

	private CurrencyHistory InsertMissingValues(CurrencyHistory currencyHistory)
	{
		if (currencyHistory.history.Count == 0)
		{
			return currencyHistory;
		}
		if (currencyHistory.history.First().date != currencyHistory.startDate)
		{
			DatePriceOHLC newPrice = new DatePriceOHLC(
				currencyHistory.startDate,
				currencyHistory.history.First().openPrice,
				currencyHistory.history.First().highPrice,
				currencyHistory.history.First().lowPrice,
				currencyHistory.history.First().closePrice
			);
			currencyHistory.history.Insert(0, newPrice);
		}
		for (int i = 0; i < currencyHistory.history.Count - 1; i++)
		{
			if (currencyHistory.history[i].date.AddDays(1) != currencyHistory.history[i + 1].date)
			{
				DatePriceOHLC newPrice = new DatePriceOHLC(
					currencyHistory.history[i].date.AddDays(1),
					currencyHistory.history[i].openPrice,
					currencyHistory.history[i].highPrice,
					currencyHistory.history[i].lowPrice,
					currencyHistory.history[i].closePrice
				);
				currencyHistory.history.Insert(i + 1, newPrice);
			}
		}
		return currencyHistory;
	}

	private void SaveCurrencyHistory(Data.CurrencyHistory history, bool updateStartTrackingDate, bool updateEndTrackingDate)
	{
		if (history.history.Count == 0)
			return;
		String insertIntoCurrencyRatesQuery = "EXEC BulkJsonCurrencyRates @CurrencyRatesBulk, @Code";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(insertIntoCurrencyRatesQuery, connection);
		command.Parameters.AddWithValue("@CurrencyRatesBulk", JsonConvert.SerializeObject(history.history));
		command.Parameters.AddWithValue("@Code", history.currency);
		try
		{
			command.ExecuteNonQuery();

		}
		catch (Exception e)
		{
			System.Console.WriteLine("Start tracking date: " + history.history.First().date);
			System.Console.WriteLine("End tracking date: " + history.history.Last().date);
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "There was a problem when adding currency rates to the database");
		}
	}
}