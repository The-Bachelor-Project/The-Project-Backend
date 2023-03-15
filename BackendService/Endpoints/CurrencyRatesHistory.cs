using System.Data.SqlClient;

namespace BackendService;

class CurrencyRatesHistory
{
	public static async Task<CurrencyRatesHistoryResponse> endpoint(CurrencyRatesHistoryBody body)
	{
		try
		{
			Data.CurrencyHistory CurrencyHistory = new Data.CurrencyHistory(body.currency.ToUpper(), body.start_date, body.end_date, body.interval);
			return new CurrencyRatesHistoryResponse("success", await DatabaseService.CurrencyRatesHistory.Get(CurrencyHistory));
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e.StackTrace);
			throw e;
		}
	}
}

class CurrencyRatesHistoryResponse
{
	public CurrencyRatesHistoryResponse(string response, Data.CurrencyHistory history)
	{
		this.Response = response;
		this.History = history;
	}

	public String Response { get; set; }
	public Data.CurrencyHistory History { get; set; }
}

class CurrencyRatesHistoryBody
{
	public CurrencyRatesHistoryBody(String currency, String start_date, String end_date, String interval)
	{
		this.currency = currency;
		this.start_date = start_date;
		this.end_date = end_date;
		this.interval = interval;
	}

	public String currency { get; set; }
	public String start_date { get; set; }
	public String end_date { get; set; }
	public String interval { get; set; }
}