using System.Data.SqlClient;

namespace Authentication;

class RefreshTokens
{
	public static BusinessLogic.TokenSet all(String refreshToken)
	{
		ValidFunctionResponse ValidFunctionResponse = IsRefreshValid(refreshToken);
		if (ValidFunctionResponse.isValid == 1)
		{
			BusinessLogic.TokenSet TokenSet = SetupTokens.call(ValidFunctionResponse.userID, ValidFunctionResponse.familyID);
			return TokenSet;
		}
		else if (ValidFunctionResponse.isValid == 0)
		{
			//FIXME: Error not being handled
			BusinessLogic.TokenSet TokenSet = new BusinessLogic.TokenSet("is_expired", "");
			return TokenSet;
		}
		else
		{
			//FIXME: Error not being handled
			BusinessLogic.TokenSet TokenSet = new BusinessLogic.TokenSet("error", "");
			return TokenSet;
		}
	}

	public static ValidFunctionResponse IsRefreshValid(String refreshToken)
	{
		String CheckIfValidQuery = "SELECT * FROM CheckIfRefreshIsValid(@RefreshToken, @UnixNow) AS IsValid";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(CheckIfValidQuery, Connection);
		Command.Parameters.AddWithValue("@RefreshToken", refreshToken);
		Command.Parameters.AddWithValue("@UnixNow", Tools.TimeConverter.dateTimeToUnix(DateTime.Now));
		try
		{
			SqlDataReader Reader = Command.ExecuteReader();
			if (Reader.Read())
			{
				int IsValid = int.Parse(Reader["IsValid"].ToString()!);
				if (IsValid == 1)
				{
					return new ValidFunctionResponse(int.Parse(Reader["IsValid"].ToString()!), int.Parse(Reader["FamilyID"].ToString()!), Reader["UserID"].ToString()!);
				}
				else
				{
					InvalidateFamily(int.Parse(Reader["FamilyID"].ToString()!));
					return new ValidFunctionResponse(0, 0, "");
				}
			}
			else
			{
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
		String InvalidateFamilyQuery = "UPDATE TokenFamily SET valid = 0 WHERE id = @family_id";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(InvalidateFamilyQuery, Connection);
		Command.Parameters.AddWithValue("@family_id", familyID);
		try
		{
			Command.ExecuteNonQuery();
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