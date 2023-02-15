namespace Data;

class StockProfile
{
	public String? Ticker { get; set; }
	public String? Exchange { get; set; }
	public String? Name { get; set; }
	public String? Industry { get; set; }
	public String? Sector { get; set; }
	public String? Website { get; set; }
	public String? Country { get; set; }
	int trackingDate;

	public StockProfile() { }
	public StockProfile(string? ticker, string? exchange, string? name)
	{
		Ticker = ticker;
		Exchange = exchange;
		Name = name;

	}
}