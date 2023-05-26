class StatusCodeException : Exception
{
	public int StatusCode { get; set; }
	public StatusCodeException(int statusCode) : base()
	{
		this.StatusCode = statusCode;
	}
	public StatusCodeException(int statusCode, string message) : base(message)
	{
		this.StatusCode = statusCode;
	}
}