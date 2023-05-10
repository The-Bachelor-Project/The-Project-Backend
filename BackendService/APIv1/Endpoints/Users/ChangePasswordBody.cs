public class ChangePasswordBody
{
	public ChangePasswordBody(String oldPassword, String newPassword, String email)
	{
		this.oldPassword = oldPassword;
		this.newPassword = newPassword;
		this.email = email;
	}
	public String oldPassword { get; set; }
	public String newPassword { get; set; }
	public String email { get; set; }
}