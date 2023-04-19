namespace Data;



public class StockHistory
{
	public StockHistory(string ticker, string exchange, DateOnly startDate, DateOnly endDate, String interval)
	{
		this.Ticker = ticker;
		this.Exchange = exchange;
		this.StartDate = startDate;
		this.EndDate = endDate;
		this.Interval = interval;
		this.History = new List<Data.DatePrice>();
	}

	public StockHistory(string ticker, string exchange, String interval)
	{
		this.Ticker = ticker;
		this.Exchange = exchange;
		this.Interval = interval;
		this.History = new List<Data.DatePrice>();
	}

	public String Ticker { get; set; }
	public String Exchange { get; set; }
	public DateOnly? StartDate { get; set; }
	public DateOnly? EndDate { get; set; }
	public String Interval { get; set; }
	public List<Data.DatePrice> History { get; set; }
}