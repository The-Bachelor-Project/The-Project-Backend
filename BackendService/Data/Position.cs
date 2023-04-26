namespace Data;

public class Position
{
	public String ticker { get; set; }
	public String exchange { get; set; }
	public List<DatePrice> valueHistory { get; set; }

	public Position(String ticker, String exchange, List<DatePrice> valueHistory)
	{
		this.ticker = ticker;
		this.exchange = exchange;
		this.valueHistory = valueHistory;
	}
}