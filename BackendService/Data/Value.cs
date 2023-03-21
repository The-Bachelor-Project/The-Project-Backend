namespace Data;

class Value
{
	public Value(decimal amount, String currency)
	{
		Amount = amount;
		Currency = currency;
	}

	public Decimal Amount { get; set; }
	public String Currency { get; set; }
}