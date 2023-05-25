class CouldNotGetStockTransactionException : Exception
{
	public CouldNotGetStockTransactionException()
	{ }

	public CouldNotGetStockTransactionException(string message)
		: base(message)
	{ }

	public CouldNotGetStockTransactionException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}