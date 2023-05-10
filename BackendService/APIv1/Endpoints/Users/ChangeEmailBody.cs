namespace API.v1;

public class ChangeEmailBody
{
	public ChangeEmailBody(String oldEmail, String newEmail)
	{
		this.oldEmail = oldEmail;
		this.newEmail = newEmail;
	}

	public String oldEmail { get; set; }
	public String newEmail { get; set; }
}