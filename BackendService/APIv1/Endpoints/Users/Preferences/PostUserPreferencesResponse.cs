namespace API.v1;

public class PostUserPreferencesResponse
{


	public String response { get; set; }
	public int id { get; set; }

	public PostUserPreferencesResponse(string response, int id)
	{
		this.response = response;
		this.id = id;
	}
}

