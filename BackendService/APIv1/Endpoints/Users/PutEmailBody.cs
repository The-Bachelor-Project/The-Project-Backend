namespace API.v1;

public class PutEmailBody
{
	public PutEmailBody(String newEmail)
	{
		this.newEmail = newEmail;
	}

	public String newEmail { get; set; }
}