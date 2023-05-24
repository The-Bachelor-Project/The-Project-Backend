namespace API.v1;

public class GetStockProfilesResponse
{
	public GetStockProfilesResponse(string response, Data.StockProfile stock)
	{
		this.response = response;
		this.stock = stock;
	}
	public String response { get; set; }
	public Data.StockProfile? stock { get; set; }
	public String? error { get; set; }
}