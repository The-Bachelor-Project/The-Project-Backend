namespace StockApp;

public class Stock
{
	public Stock(string ticker, string exchange)
	{
		this.ticker = ticker;
		this.exchange = exchange;
	}
	public string ticker { get; set; }
	public string exchange { get; set; }
}