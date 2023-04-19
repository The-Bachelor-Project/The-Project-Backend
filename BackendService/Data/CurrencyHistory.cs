namespace Data;



public class CurrencyHistory
{
	public CurrencyHistory(String Currency, DateOnly StartDate, DateOnly EndDate, String Interval)
	{
		this.Currency = Currency;
		this.StartDate = StartDate;
		this.EndDate = EndDate;
		this.Interval = Interval;
		this.History = new List<Data.DatePrice>();
	}

	public String Currency { get; set; }
	public DateOnly StartDate { get; set; }
	public DateOnly EndDate { get; set; }
	public String Interval { get; set; }
	public List<Data.DatePrice> History { get; set; }
}