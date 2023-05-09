namespace Data;



public class StockHistory
{
	public StockHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, String interval)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.startDate = startDate;
		this.endDate = endDate;
		this.interval = interval;
		this.history = new List<Data.DatePriceOHLC>();
		this.dividends = new List<Data.Dividend>();
	}

	public StockHistory(string ticker, string exchange, String interval)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.interval = interval;
		this.history = new List<Data.DatePriceOHLC>();
		this.dividends = new List<Data.Dividend>();
	}

	public String ticker { get; set; }
	public String exchange { get; set; }
	public DateOnly? startDate { get; set; }
	public DateOnly? endDate { get; set; }
	public String interval { get; set; }
	public List<Data.DatePriceOHLC> history { get; set; }
	public List<Data.Dividend> dividends { get; set; }
}