namespace Data;

public class CashBalance
{
	public DateOnly date { get; set; }
	public StockApp.Money balance { get; set; }

	public CashBalance(DateOnly date, StockApp.Money balance)
	{
		this.date = date;
		this.balance = balance;
	}

}