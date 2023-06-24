namespace API.v1;
public class PostStockTransactionsBody
{
	public String portfolioId { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
	public Decimal amount { get; set; }
	public int timestamp { get; set; }
	public StockApp.Money priceNative { get; set; }

	public PostStockTransactionsBody(String portfolioId, String ticker, String exchange, Decimal amount, int timestamp, StockApp.Money priceNative)
	{
		this.portfolioId = portfolioId;
		this.ticker = ticker;
		this.exchange = exchange;
		this.amount = amount;
		this.timestamp = timestamp;
		this.priceNative = priceNative;
	}

}