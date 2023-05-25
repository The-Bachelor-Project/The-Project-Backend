class UpdatePortfolioException : Exception
{
	public UpdatePortfolioException()
	{ }

	public UpdatePortfolioException(string message)
		: base(message)
	{ }

	public UpdatePortfolioException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}
