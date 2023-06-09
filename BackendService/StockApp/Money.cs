using System.Text.Json.Serialization;

namespace StockApp;

public class Money
{
	public const string DEFAULT_CURRENCY = "USD";

	public decimal amount { get; set; }
	public string currency { get; set; }

	[JsonConstructor]
	public Money(decimal amount, string currency)
	{
		this.amount = amount;
		this.currency = currency;
	}
}