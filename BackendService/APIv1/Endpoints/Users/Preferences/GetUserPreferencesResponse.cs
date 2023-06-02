namespace API.v1;

public class GetUserPreferencesResponse
{
	public Dictionary<String, String> preferences { get; set; }
	public String response { get; set; }

	public GetUserPreferencesResponse(String response, Dictionary<String, String> preferences)
	{
		this.preferences = preferences;
		this.response = response;
	}
}
