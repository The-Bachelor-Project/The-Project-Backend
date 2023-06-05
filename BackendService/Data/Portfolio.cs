namespace Data;

public class Portfolio
{
	public String name { get; set; }
	public String currency { get; set; }
	public List<DatePriceOHLC> valueHistory { get; set; }
	public List<Position> positions { get; set; }
	public List<Dividend> dividendHistory { get; set; }
	public List<CashBalance> cashBalance { get; set; }

	public Portfolio(String name, String currency, List<DatePriceOHLC> valueHistory, List<Position> positions, List<Dividend> dividendHistory, List<CashBalance> cashBalance)
	{
		this.name = name;
		this.currency = currency;
		this.valueHistory = valueHistory;
		this.positions = positions;
		this.dividendHistory = dividendHistory;
		this.cashBalance = cashBalance;
	}
}