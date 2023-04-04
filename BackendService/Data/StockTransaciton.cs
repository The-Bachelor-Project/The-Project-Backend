namespace Data;

public class StockTransaction
{
	public StockTransaction(string portfolio, string ticker, string exchange, decimal amount, int timestamp, Data.Value price)
	{
		this.portfolio = portfolio;
		this.ticker = ticker;
		this.exchange = exchange;
		this.amount = amount;
		this.timestamp = timestamp;
		this.price = price;
	}

	public String portfolio { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
	public Decimal amount { get; set; }
	public int timestamp { get; set; }
	public Data.Value price { get; set; }
}