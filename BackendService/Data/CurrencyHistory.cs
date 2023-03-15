namespace Data;



class CurrencyHistory
{
	public CurrencyHistory(String Currency, String StartDate, String EndDate, String Interval)
	{
		this.Currency = Currency;
		this.StartDate = StartDate;
		this.EndDate = EndDate;
		this.Interval = Interval;
		this.History = new CurrencyHistoryData[] { };
	}

	public String Currency { get; set; }
	public String StartDate { get; set; }
	public String EndDate { get; set; }
	public String Interval { get; set; }
	public CurrencyHistoryData[] History { get; set; }
}

class CurrencyHistoryData
{
	public CurrencyHistoryData(String Date, Decimal OpenPrice, Decimal HighPrice, Decimal LowPrice, Decimal ClosePrice)
	{
		this.Date = Date;
		this.OpenPrice = OpenPrice;
		this.HighPrice = HighPrice;
		this.LowPrice = LowPrice;
		this.ClosePrice = ClosePrice;
	}

	public String Date { get; set; }
	public Decimal OpenPrice { get; set; }
	public Decimal HighPrice { get; set; }
	public Decimal LowPrice { get; set; }
	public Decimal ClosePrice { get; set; }
}