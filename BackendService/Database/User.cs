using System.Data.SqlClient;

namespace DatabaseService;

class User
{
	public static String SignIn(String email, String password)
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

	public static String SignUp(String email, String password)
	{
		SqlConnection Connection = Database.createConnection();
		String GetEmailQuery = "SELECT email FROM Accounts WHERE email = @email";
		SqlCommand Command = new SqlCommand(GetEmailQuery, Connection);
		Command.Parameters.AddWithValue("@email", email);
		SqlDataReader Reader = Command.ExecuteReader();
		if (!Reader.Read())
		{
			Reader.Close();
			String UID = RandomString.Generate(32);
			String Query = "INSERT INTO Accounts (user_id, email, password) VALUES (@user_id, @email, @password)";
			Command = new SqlCommand(Query, Connection);
			Command.Parameters.AddWithValue("@user_id", UID);
			Command.Parameters.AddWithValue("@email", email);
			Command.Parameters.AddWithValue("@password", password);
			try
			{
				Command.ExecuteNonQuery();
				return UID;
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
			}
		}
		throw new UserAlreadyExist();
	}
}