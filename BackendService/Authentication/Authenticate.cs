using System.Data.SqlClient;

namespace Authentication;

public class Authenticate
{
	public static bool AccessToken(string accessToken)
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "SELECT dbo.CheckIfAccessIsValid(@access_token, @UnixNow) AS IsValid";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@access_token", accessToken);
		command.Parameters.AddWithValue("@UnixNow", Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		using (SqlDataReader reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				Boolean isValid = Boolean.Parse(reader["IsValid"].ToString()!);
				reader.Close();
				if (isValid)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				reader.Close();
				return false;
			}
		}

	}
}