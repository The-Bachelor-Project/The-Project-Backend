class UnauthorizedAccess : Exception
{
	public UnauthorizedAccess()
	{ }

	public UnauthorizedAccess(string message)
		: base(message)
	{ }

	public UnauthorizedAccess(string message, Exception innerException)
		: base(message, innerException)
	{ }
}
