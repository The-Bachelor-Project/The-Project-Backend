using System.Data.SqlClient;
using Data.Interfaces;

namespace Data.YahooFinance;

class CurrencyHistory : ICurrencyHistory
{
	public async Task<Data.CurrencyHistory> Usd(String currency, DateOnly startDate, DateOnly endDate)
	{
		int StartTime = Tools.TimeConverter.dateOnlyToUnix(startDate);
		int EndTime = Tools.TimeConverter.dateOnlyToUnix(endDate);

		HttpClient Client = new HttpClient();

		String url = "https://query1.finance.yahoo.com/v7/finance/download/" + currency + "USD=X" + "?interval=1d&period1=" + StartTime + "&period2=" + EndTime;

		HttpResponseMessage StockHistoryRes = await Client.GetAsync(url);
		String StockHistoryCsv = await StockHistoryRes.Content.ReadAsStringAsync();

		String[] DataLines = StockHistoryCsv.Replace("\r", "").Split("\n");

		Data.CurrencyHistory Result = new Data.CurrencyHistory(currency, startDate, endDate, "daily");
		String InsertIntoCurrencyRatesQuery = "INSERT INTO CurrencyRatesUSD VALUES (@code, @date, @open_price, @high_price, @low_price, @close_price)";
		foreach (String Line in DataLines)
		{
			String[] Data = Line.Split(",");
			Result.History = Result.History.Append(new Data.CurrencyHistoryData(DateOnly.Parse(Data[0]), Decimal.Parse(Data[1]), Decimal.Parse(Data[2]), Decimal.Parse(Data[3]), Decimal.Parse(Data[4]))).ToArray();

			using (SqlConnection Connection = new Database.Connection().Create())
			{
				//FIXME TODO Look into using a BULK INSERT query, you go Frederik
				SqlCommand Command = new SqlCommand(InsertIntoCurrencyRatesQuery, Connection);
				Command.Parameters.AddWithValue("@code", currency);
				Command.Parameters.AddWithValue("@date", Data[0]);
				Command.Parameters.AddWithValue("@open_price", Decimal.Parse(Data[1]));
				Command.Parameters.AddWithValue("@high_price", Decimal.Parse(Data[2]));
				Command.Parameters.AddWithValue("@low_price", Decimal.Parse(Data[3]));
				Command.Parameters.AddWithValue("@close_price", Decimal.Parse(Data[4]));
				Command.Parameters.AddWithValue("@volume", int.Parse(Data[6]));
				try
				{
					Command.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
			}
		}

		return Result;
	}
}