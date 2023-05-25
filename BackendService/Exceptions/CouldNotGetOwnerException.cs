class CouldNotGetOwnerException : Exception
{
	public CouldNotGetOwnerException()
	{ }

	public CouldNotGetOwnerException(string message)
		: base(message)
	{ }

	public CouldNotGetOwnerException(string message, Exception innerException)
		: base(message, innerException)
	{ }
}