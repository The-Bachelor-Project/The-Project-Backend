using StockApp;

namespace API.v1;

public class PostCashTransactionsBody
{

	public String? portfolio { get; set; }
	public String? currency { get; set; }
	public Decimal nativeAmount { get; set; }
	public int timestamp { get; set; }
	public String? description { get; set; }
	public PostCashTransactionsBody(String portfolio, String currency, Decimal nativeAmount, int timestamp, String? description)
	{
		this.portfolio = portfolio;
		this.currency = currency;
		this.nativeAmount = nativeAmount;
		this.timestamp = timestamp;
		this.description = description;
	}
}
