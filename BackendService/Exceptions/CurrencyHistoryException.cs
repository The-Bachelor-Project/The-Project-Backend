class CurrencyHistoryException : Exception
{
	public CurrencyHistoryException()
	{ }

	public CurrencyHistoryException(string message)
		: base(message)
	{ }

	public CurrencyHistoryException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}