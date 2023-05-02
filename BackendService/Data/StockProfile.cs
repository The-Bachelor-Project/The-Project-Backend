namespace Data;

public class StockProfile
{
	public String? ticker { get; set; }
	public String? exchange { get; set; }
	public String? displayName { get; set; }
	public String? shortName { get; set; }
	public String? longName { get; set; }
	public String? industry { get; set; }
	public String? sector { get; set; }
	public String? website { get; set; }
	public String? country { get; set; }
	public String? address { get; set; }
	public String? city { get; set; }
	public String? state { get; set; }
	public String? zip { get; set; }
	public String? financialCurrency { get; set; }
	public Decimal? sharesOutstanding { get; set; }
	int trackingDate;

	public StockProfile() { }
	public StockProfile(string? ticker, string? exchange, string? name)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.displayName = name;

	}
}