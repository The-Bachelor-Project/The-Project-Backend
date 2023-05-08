namespace Data;

public class UserAssetsValueHistory
{
	public List<DatePriceOHLC> valueHistory { get; set; }
	public List<Portfolio> portfolios { get; set; }

	public UserAssetsValueHistory(List<DatePriceOHLC> valueHistory, List<Portfolio> portfolios)
	{
		this.valueHistory = valueHistory;
		this.portfolios = portfolios;
	}
}