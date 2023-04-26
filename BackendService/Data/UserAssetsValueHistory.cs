namespace Data;

public class UserAssetsValueHistory
{
	public List<DatePrice> valueHistory { get; set; }
	public List<Portfolio> portfolios { get; set; }

	public UserAssetsValueHistory(List<DatePrice> valueHistory, List<Portfolio> portfolios)
	{
		this.valueHistory = valueHistory;
		this.portfolios = portfolios;
	}
}