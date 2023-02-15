namespace Data;

class Value
{
	public Value(decimal amount, Currency currency)
	{
		Amount = amount;
		Currency = currency;
	}

	public Decimal Amount { get; set; }
	public Currency Currency { get; set; }
}