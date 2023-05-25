namespace API.v1;

public class PutUserResponse
{
	public PutUserResponse(String response)
	{
		this.response = response;
	}
	public String response { get; set; }
}