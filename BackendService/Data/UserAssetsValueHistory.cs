namespace Data;

public class UserAssetsValueHistory
{
	public List<DatePriceOHLC> valueHistory { get; set; }
	public List<Dividend> dividendHistory { get; set; }
	public List<Portfolio> portfolios { get; set; }

	public UserAssetsValueHistory(List<DatePriceOHLC> valueHistory, List<Portfolio> portfolios, List<Dividend> dividendHistory)
	{
		this.valueHistory = valueHistory;
		this.portfolios = portfolios;
		this.dividendHistory = dividendHistory;
	}
}