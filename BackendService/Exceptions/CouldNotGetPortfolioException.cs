class CouldNotGetPortfolioException : Exception
{
	public CouldNotGetPortfolioException()
	{ }

	public CouldNotGetPortfolioException(string message)
		: base(message)
	{ }

	public CouldNotGetPortfolioException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}