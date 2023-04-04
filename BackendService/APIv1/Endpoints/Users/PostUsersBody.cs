namespace API.v1.Endpoints;

public class PostUsersBody
{
	public PostUsersBody(string email, string password)
	{
		this.email = email;
		this.password = password;
	}

	public String email { get; set; }
	public String password { get; set; }
}
