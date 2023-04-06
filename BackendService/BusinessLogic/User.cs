namespace BusinessLogic;

class User
{
	public String? Id { get; set; }
	public String Email { get; set; }
	public String Password { get; set; }

	public User(string email, string password)
	{
		this.Email = email;
		this.Password = password;
	}
	

	public User SignUp()
	{
		try
		{
			Id = DatabaseService.User.SignUp(Email, Password);
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
		}

		return this;
	}

	public User SignIn()
	{
		Id = DatabaseService.User.GetUserId(Email, Password);
		return this;
	}
}