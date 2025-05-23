namespace Data;

public class Position
{
	public String ticker { get; set; }
	public String exchange { get; set; }
	public List<DatePriceOHLC> valueHistory { get; set; }
	public List<Dividend> dividends { get; set; }

	public Position(String ticker, String exchange, List<DatePriceOHLC> valueHistory, List<Dividend> dividends)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.valueHistory = valueHistory;
		this.dividends = dividends;
	}
}