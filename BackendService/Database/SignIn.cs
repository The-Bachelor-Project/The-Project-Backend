using System.Data.SqlClient;

namespace DatabaseService;

class SignIn
{
	public static String Execute(String email, String password)
	{
		SqlConnection Connection = Database.createConnection();
		String Query = "SELECT * FROM Accounts WHERE email = @email";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@email", email);
		SqlDataReader Reader = Command.ExecuteReader();
		if (Reader.Read())
		{
			String? DbPassword = Reader["password"].ToString();
			Reader.Close();
			//TODO Check password
			String Token = RandomString.Generate(32);
			return Token; //TODO do more with token
		}
		throw new UserDoesNotExistException("No user with the email \"" + email + "\" was found");
	}
}