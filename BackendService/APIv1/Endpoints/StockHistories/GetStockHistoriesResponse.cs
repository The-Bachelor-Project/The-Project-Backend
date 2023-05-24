namespace API.v1;

public class GetStockHistoriesResponse
{
	public GetStockHistoriesResponse(string response, Data.StockHistory history)
	{
		this.response = response;
		this.history = history;
	}

	public String response { get; set; }
	public Data.StockHistory history { get; set; }
	public String? error { get; set; }
}