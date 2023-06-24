namespace API.v1;

public class PutCashTransactionsBody
{
	public int id { get; set; }
	public String portfolio { get; set; }
	public String newNativeCurrency { get; set; }
	public Decimal newNativeAmount { get; set; }
	public String newDescription { get; set; }
	public int newTimestamp { get; set; }

	public PutCashTransactionsBody(int id, String portfolio, String newNativeCurrency, Decimal newNativeAmount, String newDescription, int newTimestamp)
	{
		this.id = id;
		this.portfolio = portfolio;
		this.newNativeCurrency = newNativeCurrency;
		this.newNativeAmount = newNativeAmount;
		this.newDescription = newDescription;
		this.newTimestamp = newTimestamp;
	}

}
