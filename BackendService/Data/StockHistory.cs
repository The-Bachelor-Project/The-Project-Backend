namespace Data;



public class StockHistory
{
	public StockHistory(string ticker, string exchange, String startDate, String endDate, String interval)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.startDate = startDate;
		this.endDate = endDate;
		this.interval = interval;
		this.history = new StockHistoryData[] { };
	}

	public String ticker { get; set; }
	public String exchange { get; set; }
	public String startDate { get; set; }
	public String endDate { get; set; }
	public String interval { get; set; }
	public StockHistoryData[] history { get; set; }
}

public class StockHistoryData
{
	public StockHistoryData(string date, decimal open_price, decimal high_price, decimal low_price, decimal close_price)
	{
		this.date = date;
		this.open_price = open_price;
		this.high_price = high_price;
		this.low_price = low_price;
		this.close_price = close_price;
	}

	public String date { get; set; }
	public Decimal open_price { get; set; }
	public Decimal high_price { get; set; }
	public Decimal low_price { get; set; }
	public Decimal close_price { get; set; }
}