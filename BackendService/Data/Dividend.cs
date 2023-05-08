namespace Data;

public class Dividend
{
	public Dividend(DateOnly date, Money payout)
	{
		this.date = date;
		this.payout = payout;
	}

	public DateOnly date { get; set; }
	public Money payout { get; set; }
}