namespace API.v1;

public class DeletePortfoliosResponse
{
	public string response { get; set; }
	public DeletePortfoliosResponse(string response)
	{
		this.response = response;
	}
}