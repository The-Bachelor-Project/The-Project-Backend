namespace Data;



public class CurrencyHistory
{
	public CurrencyHistory(String Currency, DateOnly StartDate, DateOnly EndDate, String Interval)
	{
		this.Currency = Currency;
		this.StartDate = StartDate;
		this.EndDate = EndDate;
		this.Interval = Interval;
		this.History = new CurrencyHistoryData[] { };
	}

	public String Currency { get; set; }
	public DateOnly StartDate { get; set; }
	public DateOnly EndDate { get; set; }
	public String Interval { get; set; }
	public CurrencyHistoryData[] History { get; set; }
}

public class CurrencyHistoryData
{
	public CurrencyHistoryData(DateOnly Date, Decimal OpenPrice, Decimal HighPrice, Decimal LowPrice, Decimal ClosePrice)
	{
		this.Date = Date;
		this.OpenPrice = OpenPrice;
		this.HighPrice = HighPrice;
		this.LowPrice = LowPrice;
		this.ClosePrice = ClosePrice;
	}

	public DateOnly Date { get; set; }
	public Decimal OpenPrice { get; set; }
	public Decimal HighPrice { get; set; }
	public Decimal LowPrice { get; set; }
	public Decimal ClosePrice { get; set; }
}