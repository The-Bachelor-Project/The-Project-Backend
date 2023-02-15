using System.Data.SqlClient;

namespace DatabaseService;

class SignUp
{
	public static String Execute(String email, String password)
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