namespace Data;

public class StockTransaction //TODO Delete this file, use the one on BusinessLogic instead
{
	public StockTransaction(int id, string portfolio, string ticker, string exchange, decimal amount, int timestamp, Money price)
	{
		this.id = id;
		this.portfolio = portfolio;
		this.ticker = ticker;
		this.exchange = exchange;
		this.amount = amount;
		this.timestamp = timestamp;
		this.price = price;
	}

	public int id { get; set; }
	public String portfolio { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
	public Decimal amount { get; set; }
	public int timestamp { get; set; }
	public Money price { get; set; }
}