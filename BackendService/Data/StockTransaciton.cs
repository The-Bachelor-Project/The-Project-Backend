namespace Data;

public class StockTransaction //TODO Delete this file, use the one on BusinessLogic instead
{
	public StockTransaction(string portfolio, string ticker, string exchange, decimal amount, int timestamp, Decimal price, string currency)
	{
		this.portfolio = portfolio;
		this.ticker = ticker;
		this.exchange = exchange;
		this.amount = amount;
		this.timestamp = timestamp;
		this.price = price;
		this.currency = currency;
	}

	public String portfolio { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
	public Decimal amount { get; set; }
	public int timestamp { get; set; }
	public Decimal price { get; set; }
	public String currency { get; set; }
}