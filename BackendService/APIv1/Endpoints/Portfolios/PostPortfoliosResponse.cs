namespace API.v1;

public class PostPortfoliosResponse
{
	public PostPortfoliosResponse(string response, string id)
	{
		this.response = response;
		this.id = id;
	}

	public String response { get; set; }
	public String id { get; set; }
}