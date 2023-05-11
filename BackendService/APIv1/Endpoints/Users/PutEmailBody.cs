namespace API.v1;

public class PutEmailBody
{
	public PutEmailBody(String oldEmail, String newEmail)
	{
		this.oldEmail = oldEmail;
		this.newEmail = newEmail;
	}

	public String oldEmail { get; set; }
	public String newEmail { get; set; }
}