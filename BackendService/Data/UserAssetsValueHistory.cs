namespace Data;

public class UserAssetsValueHistory
{
	public List<DatePriceOHLC> valueHistory { get; set; }
	public List<Dividend> dividendHistory { get; set; }
	public List<StockApp.Portfolio> portfolios { get; set; }
	public List<Data.CashBalance> cashBalanceHistory { get; set; }

	public UserAssetsValueHistory(List<DatePriceOHLC> valueHistory, List<StockApp.Portfolio> portfolios, List<Dividend> dividendHistory, List<Data.CashBalance> cashBalance)
	{
		this.valueHistory = valueHistory;
		this.portfolios = portfolios;
		this.dividendHistory = dividendHistory;
		this.cashBalanceHistory = cashBalance;
	}
}