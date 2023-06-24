namespace API.v1;

public class PutEmailBody
{
	public PutEmailBody(String newEmail, String password)
	{
		this.newEmail = newEmail;
		this.password = password;
	}

	public String newEmail { get; set; }
	public String password { get; set; }
}