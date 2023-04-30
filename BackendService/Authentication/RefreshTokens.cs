using System.Data.SqlClient;

namespace Authentication;

class RefreshTokens
{
	public static StockApp.TokenSet All(String refreshToken)
	{
		ValidFunctionResponse validFunctionResponse = IsRefreshValid(refreshToken);
		if (validFunctionResponse.isValid == 1)
		{
			StockApp.TokenSet tokenSet = SetupTokens.Call(validFunctionResponse.userID, validFunctionResponse.familyID);
			return tokenSet;
		}
		else if (validFunctionResponse.isValid == 0)
		{
			//FIXME: Error not being handled
			StockApp.TokenSet tokenSet = new StockApp.TokenSet("is_expired", "");
			return tokenSet;
		}
		else
		{
			//FIXME: Error not being handled
			StockApp.TokenSet tokenSet = new StockApp.TokenSet("error", "");
			return tokenSet;
		}
	}

	public static ValidFunctionResponse IsRefreshValid(String refreshToken)
	{
		String checkIfValidQuery = "SELECT * FROM CheckIfRefreshIsValid(@RefreshToken, @UnixNow) AS IsValid";
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand(checkIfValidQuery, connection);
		command.Parameters.AddWithValue("@RefreshToken", refreshToken);
		command.Parameters.AddWithValue("@UnixNow", Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		try
		{
			SqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				int isValid = int.Parse(reader["IsValid"].ToString()!);
				if (isValid == 1)
				{
					reader.Close();
					return new ValidFunctionResponse(int.Parse(reader["IsValid"].ToString()!), int.Parse(reader["FamilyID"].ToString()!), reader["UserID"].ToString()!);
				}
				else
				{
					reader.Close();
					InvalidateFamily(int.Parse(reader["FamilyID"].ToString()!));
					return new ValidFunctionResponse(0, 0, "");
				}
			}
			else
			{
				reader.Close();
				return new ValidFunctionResponse(-1, 0, "");
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

	public class ValidFunctionResponse
	{
		public ValidFunctionResponse(int isValid, int familyID, String userID)
		{
			this.isValid = isValid;
			this.familyID = familyID;
			this.userID = userID;
		}
		public int isValid { get; set; }
		public int familyID { get; set; }
		public String userID { get; set; }
	}
}