class StockProfileException : Exception
{
	public StockProfileException()
	{ }

	public StockProfileException(string message)
		: base(message)
	{ }

	public StockProfileException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}