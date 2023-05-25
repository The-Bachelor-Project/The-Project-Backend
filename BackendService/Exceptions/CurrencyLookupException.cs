class CurrencyLookupException : Exception
{
	public CurrencyLookupException()
	{ }

	public CurrencyLookupException(string message)
		: base(message)
	{ }

	public CurrencyLookupException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}