namespace Data;

public class Dividend
{
	public Dividend(DateOnly date, StockApp.Money payout)
	{
		this.date = date;
		this.payout = payout;
	}

	public DateOnly date { get; set; }
	public StockApp.Money payout { get; set; }
}