class UserAlreadyExist : Exception
{
	public UserAlreadyExist()
	{ }

	public UserAlreadyExist(string message)
		: base(message)
	{ }

	public UserAlreadyExist(string message, Exception innerException)
		: base(message, innerException)
	{ }
}