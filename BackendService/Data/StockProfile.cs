namespace Data;

public class StockProfile
{
	public String? ticker { get; set; }
	public String? exchange { get; set; }
	public String? name { get; set; }
	public String? industry { get; set; }
	public String? sector { get; set; }
	public String? website { get; set; }
	public String? country { get; set; }
	int trackingDate;

	public StockProfile() { }
	public StockProfile(string? ticker, string? exchange, string? name)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.name = name;

	}
}