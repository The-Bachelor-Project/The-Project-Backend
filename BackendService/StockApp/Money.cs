namespace StockApp;

public class Money
{
	public const string DEFAULT_CURRENCY = "usd";

	public decimal amount { get; set; }
	public string currency { get; set; }

	public Money(decimal amount, string currency)
	{
		this.amount = amount;
		this.currency = currency;
	}

	public Money(decimal amount) : this(amount, DEFAULT_CURRENCY)
	{
	}
}