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

	public static ValidFunctionResponse RefreshToken(String refreshToken)
	{
		String checkIfValidQuery = "SELECT * FROM CheckIfRefreshIsValid(@RefreshToken, @UnixNow) AS IsValid";
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand(checkIfValidQuery, connection);
		command.Parameters.AddWithValue("@RefreshToken", refreshToken);
		command.Parameters.AddWithValue("@UnixNow", Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		System.Console.WriteLine("RefreshToken: " + refreshToken);
		System.Console.WriteLine("UnixNow: " + Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		try
		{
			using (SqlDataReader reader = command.ExecuteReader())
			{
				if (reader.Read())
				{
					int isValid = int.Parse(reader["IsValid"].ToString()!);
					int familyID = int.Parse(reader["FamilyId"].ToString()!);
					String userID = reader["UserID"].ToString()!;
					reader.Close();
					if (isValid == 1)
					{
						return new ValidFunctionResponse(isValid, familyID, userID);
					}
					else
					{
						InvalidateFamily(familyID);
						return new ValidFunctionResponse(0, 0, "");
					}
				}
				else
				{
					reader.Close();
					return new ValidFunctionResponse(-1, 0, "");
				}
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			return new ValidFunctionResponse(-1, 0, "");
		}
	}

	private static void InvalidateFamily(int familyID)
	{
		String invalidateFamilyQuery = "UPDATE TokenFamily SET valid = 0 WHERE id = @family_id";
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand(invalidateFamilyQuery, connection);
		command.Parameters.AddWithValue("@family_id", familyID);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
		}
	}
}