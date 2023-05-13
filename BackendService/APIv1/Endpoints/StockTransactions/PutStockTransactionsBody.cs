namespace API.v1;

public class PutStockTransactionsBody
{
	public PutStockTransactionsBody(String id, String portfolio, Decimal newAmount, int newTimestamp, Decimal newPrice, String newCurrency)
	{
		this.id = id;
		this.portfolio = portfolio;
		this.newAmount = newAmount;
		this.newTimestamp = newTimestamp;
		this.newPrice = newPrice;
		this.newCurrency = newCurrency;
	}
	public String id { get; set; }
	public String portfolio { get; set; }
	public Decimal newAmount { get; set; }
	public int newTimestamp { get; set; }
	public Decimal newPrice { get; set; }
	public String newCurrency { get; set; }
}