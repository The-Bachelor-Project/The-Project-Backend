namespace Data;



public class CurrencyHistory
{
	public CurrencyHistory(String Currency, DateOnly StartDate, DateOnly EndDate, String Interval)
	{
		this.currency = Currency;
		this.startDate = StartDate;
		this.endDate = EndDate;
		this.interval = Interval;
		this.history = new List<Data.DatePriceOHLC>();
	}

	public String currency { get; set; }
	public DateOnly startDate { get; set; }
	public DateOnly endDate { get; set; }
	public String interval { get; set; }
	public List<Data.DatePriceOHLC> history { get; set; }
}