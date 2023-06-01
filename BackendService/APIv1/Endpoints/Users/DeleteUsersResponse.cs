namespace API.v1;

public class DeleteUsersResponse
{
	public string response { get; set; }

	public DeleteUsersResponse(string response)
	{
		this.response = response;
	}
}
