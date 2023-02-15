using System.Data.SqlClient;
using BackendService;

class Token
{
	public static String handleTokenGeneration(SqlConnection connection, SignInBody body)
	{
		String checkIfDeviceExists = "SELECT device FROM Tokens WHERE user_id = (SELECT user_id FROM Accounts WHERE email = @email) AND device = @device";
		SqlCommand command = new SqlCommand(checkIfDeviceExists, connection);
		command.Parameters.AddWithValue("@email", body.email);
		command.Parameters.AddWithValue("@device", body.device);
		SqlDataReader reader = command.ExecuteReader();
		String uid = DatabaseService.RandomString.Generate(32);
		if (reader.Read())
		{
			reader.Close();
			String updateTokenOnDevice = "UPDATE Tokens SET token = @uid WHERE user_id = (SELECT user_id FROM Accounts WHERE email = @email) AND device = @device";
			command = new SqlCommand(updateTokenOnDevice, connection);
			command.Parameters.AddWithValue("@uid", uid);
			command.Parameters.AddWithValue("@email", body.email);
			command.Parameters.AddWithValue("@device", body.device);
			try
			{
				command.ExecuteNonQuery();
				return uid;
			}
			catch (Exception)
			{
				return "";
			}
		}
		else
		{
			reader.Close();
			String insertTokenOnDevice = "INSERT INTO Tokens(user_id, device, token) VALUES ((SELECT user_id FROM Accounts WHERE email = @email), @device, @uid)";
			command = new SqlCommand(insertTokenOnDevice, connection);
			command.Parameters.AddWithValue("@email", body.email);
			command.Parameters.AddWithValue("@device", body.device);
			command.Parameters.AddWithValue("@uid", uid);
			try
			{
				command.ExecuteNonQuery();
				return uid;
			}
			catch (Exception)
			{
				return "";
			}
		}
	}
}