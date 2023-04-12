using System.Data.SqlClient;
using Data;
using DatabaseService;

class CurrencyConverter
{
	public static async Task<Dictionary<String, CurrencyHistoryData>> GetRatesAsync(DateOnly startDate, String currency)
	{
		//Dictionary<String, CurrencyHistoryData> Rates = new Dictionary<String, CurrencyHistoryData>();
		//String Today = DateOnly.FromDateTime(DateTime.Now).ToString();
		//CurrencyHistory CurrencyHistory = new CurrencyHistory(currency, startDate.ToString(), Today, "daily");
		//CurrencyHistory = await CurrencyRatesHistory.Get(CurrencyHistory);
		//for (int i = 0; i < CurrencyHistory.History.Length; i++)
		//{
		//	CurrencyHistoryData Row = CurrencyHistory.History[i];
		//	Rates.Add(Row.Date, Row);
		//}
		//return Rates;

		throw new NotImplementedException();
	}
}