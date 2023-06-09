namespace API.v1;

public class PutPasswordBody
{
	public PutPasswordBody(String oldPassword, String newPassword)
	{
		this.oldPassword = oldPassword;
		this.newPassword = newPassword;
	}
	public String oldPassword { get; set; }
	public String newPassword { get; set; }
}