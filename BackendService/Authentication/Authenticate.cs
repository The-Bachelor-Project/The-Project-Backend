using System.Data.SqlClient;

namespace Authentication;

public class Authenticate
{
	public static bool AccessToken(string accessToken)
	{
		String query = "SELECT dbo.CheckIfAccessIsValid(@access_token, @UnixNow) AS IsValid";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@access_token", accessToken);
		parameters.Add("@UnixNow", Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);
		if (data != null)
		{
			Boolean isValid = Boolean.Parse(data["IsValid"].ToString()!);
			if (isValid)
			{
				return true;
			}
		}
		return false;
	}

	public static ValidFunctionResponse RefreshToken(String refreshToken)
	{
		String checkIfValidQuery = "SELECT * FROM CheckIfRefreshIsValid(@RefreshToken, @UnixNow) AS IsValid";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@RefreshToken", refreshToken);
		parameters.Add("@UnixNow", Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(checkIfValidQuery, parameters);
		System.Console.WriteLine("RefreshToken: " + refreshToken);
		System.Console.WriteLine("UnixNow: " + Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		try
		{
			if (data != null)
			{
				int isValid = int.Parse(data["IsValid"].ToString()!);
				int familyID = int.Parse(data["FamilyId"].ToString()!);
				String userID = data["UserID"].ToString()!;
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
			return new ValidFunctionResponse(-1, 0, "");
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
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
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