class InvalidUserInput : Exception
{
	public InvalidUserInput()
	{ }

	public InvalidUserInput(string message)
		: base(message)
	{ }

	public InvalidUserInput(string message, Exception innerException)
		: base(message, innerException)
	{ }
}