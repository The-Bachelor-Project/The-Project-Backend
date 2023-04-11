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
		this.History = new StockHistoryData[] { };
	}

	public StockHistory(string ticker, string exchange, String interval)
	{
		this.Ticker = ticker;
		this.Exchange = exchange;
		this.Interval = interval;
		this.History = new StockHistoryData[] { };
	}

	public String Ticker { get; set; }
	public String Exchange { get; set; }
	public DateOnly? StartDate { get; set; }
	public DateOnly? EndDate { get; set; }
	public String Interval { get; set; }
	public StockHistoryData[] History { get; set; }
}

public class StockHistoryData
{
	public StockHistoryData(DateOnly date, decimal openPrice, decimal highPrice, decimal lowPrice, decimal closePrice)
	{
		this.Date = date;
		this.OpenPrice = openPrice;
		this.HighPrice = highPrice;
		this.LowPrice = lowPrice;
		this.ClosePrice = closePrice;
	}

	public DateOnly Date { get; set; }
	public Decimal OpenPrice { get; set; }
	public Decimal HighPrice { get; set; }
	public Decimal LowPrice { get; set; }
	public Decimal ClosePrice { get; set; }
}