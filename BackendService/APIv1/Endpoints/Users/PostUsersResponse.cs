namespace API.v1;

public class PostUsersResponse
{
	public PostUsersResponse(string response, string uid)
	{
		this.response = response;
		this.uid = uid;
	}

	public String response { get; set; }
	public String uid { get; set; }
	public String? error { get; set; }
}