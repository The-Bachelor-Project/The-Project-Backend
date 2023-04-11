namespace API.v1
{
	public class PostTokensBody
	{
		public PostTokensBody(string email, string password)
		{
			this.email = email;
			this.password = password;
		}

		public string email { get; set; }
		public string password { get; set; }
	}

}