using System.Data.SqlClient;

namespace BackendService;

class StockHistory
{
	public static async Task<StockHistoryResponse> endpoint(StockHistoryBody body)
	{
		try
		{
			Data.StockHistory stockHistory = new Data.StockHistory(body.ticker, body.exchange, body.start_date, body.end_date, body.interval);
			return new StockHistoryResponse("success", await DatabaseService.StockHistory.Get(stockHistory));
		}
		catch (Exception e)
		{
			throw e;
		}
	}
}

class StockHistoryResponse
{
	public StockHistoryResponse(string response, Data.StockHistory history)
	{
		this.response = response;
		this.history = history;
	}

	public String response { get; set; }
	public Data.StockHistory history { get; set; }
}

class StockHistoryBody
{
	public StockHistoryBody(string ticker, string exchange, String start_date, String end_date, String interval)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.start_date = start_date;
		this.end_date = end_date;
		this.interval = interval;
	}

	public String ticker { get; set; }
	public String exchange { get; set; }
	public String start_date { get; set; }
	public String end_date { get; set; }
	public String interval { get; set; }
}