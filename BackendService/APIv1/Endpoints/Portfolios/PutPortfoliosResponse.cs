namespace API.v1;

public class PutPortfoliosResponse
{
	public String response;
	public String? error { get; set; }

	public PutPortfoliosResponse(string response)
	{
		this.response = response;
	}
}
