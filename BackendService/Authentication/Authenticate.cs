using System.Data.SqlClient;

namespace Authentication;

public class Authenticate
{
	public static String AccessToken(string accessToken)
	{
		String query = "SELECT * FROM CheckIfAccessIsValid(@access_token, @UnixNow)";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@access_token", accessToken);
		parameters.Add("@UnixNow", Tools.TimeConverter.DateTimeToUnix(DateTime.Now));
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(query, parameters);

		if (data != null)
		{
			String isValid = data["is_valid"].ToString()!;
			int familyID = int.Parse(data["family_id"].ToString()!);
			if (isValid == "Valid")
			{
				return "Valid";
			}
			else if (isValid == "Expired")
			{
				return "Expired";
			}
			else if (isValid == "Invalid")
			{
				InvalidateFamily(familyID);
				return "Invalid";
			}
			else
			{
				return "Invalid";
			}
		}
		else
		{
			// If gets here, family does not exists, so token not valid, but can not invalidate family
			return "Invalid";
		}
	}

	public static RefreshAuthenticationResponse RefreshToken(String refreshToken)
	{
		String checkIfValidQuery = "SELECT * FROM CheckIfRefreshIsValid(@RefreshToken, @UnixNow)";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@RefreshToken", refreshToken);
		parameters.Add("@UnixNow", Tools.TimeConverter.DateTimeToUnix(DateTime.Now));
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(checkIfValidQuery, parameters);
		System.Console.WriteLine("RefreshToken: " + refreshToken);
		System.Console.WriteLine("UnixNow: " + Tools.TimeConverter.DateTimeToUnix(DateTime.Now));
		try
		{
			if (data != null)
			{
				int familyID = int.Parse(data["family_id"].ToString()!);
				String isValid = data["is_valid"].ToString()!;
				RefreshAuthenticationResponse response = new RefreshAuthenticationResponse(familyID);
				if (isValid == "Valid")
				{
					return response;
				}
				else if (isValid == "Expired")
				{
					response.error = "Expired";
					return response;
				}
				else
				{
					InvalidateFamily(familyID);
					response.error = "Invalid";
					return response;
				}
			}
			else
			{
				RefreshAuthenticationResponse response = new RefreshAuthenticationResponse(0);
				response.error = "Invalid";
				return response;
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			RefreshAuthenticationResponse response = new RefreshAuthenticationResponse(0);
			response.error = "Error";
			return response;
		}
	}

	private static void InvalidateFamily(int familyID)
	{
		String invalidateFamilyQuery = "DELETE FROM TokenFamily WHERE id = @family_id";
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
			throw new StatusCodeException(500, "Could not invalidate family");
		}
	}
}

public class RefreshAuthenticationResponse
{
	public int familyID { get; set; }
	public String? error { get; set; }

	public RefreshAuthenticationResponse(int familyID)
	{
		this.familyID = familyID;
	}
}