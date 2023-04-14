class CouldNotGetStockException : Exception
{
	public CouldNotGetStockException()
	{ }

	public CouldNotGetStockException(string message)
		: base(message)
	{ }

	public CouldNotGetStockException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}