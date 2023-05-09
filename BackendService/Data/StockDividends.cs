namespace Data;

public class StockDividends
{
	public StockDividends(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.startDate = startDate;
		this.endDate = endDate;
		this.dividends = new List<Data.Dividend>();
	}

	public String ticker { get; set; }
	public String exchange { get; set; }
	public DateOnly? startDate { get; set; }
	public DateOnly? endDate { get; set; }
	public List<Data.Dividend> dividends { get; set; }
}