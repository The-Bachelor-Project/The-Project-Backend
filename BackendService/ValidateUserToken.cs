using System.Data.SqlClient;

namespace BackendService;

class ValidateUserToken
{
	public static bool authenticate(String token)
	{
		// ? How do we handle device??
		String getToken = "SELECT token FROM Tokens WHERE token = @token";
		using (SqlConnection connection = Database.createConnection())
		{
			SqlCommand command = new SqlCommand(getToken, connection);
			command.Parameters.AddWithValue("@token", token);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				reader.Close();
				return true;
			}
			else
			{
				reader.Close();
				return false;
			}

		}

	}
}