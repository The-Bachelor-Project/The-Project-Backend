namespace BackendService.tests;
using System.Data.SqlClient;
using StockApp;

public class TokenHelper
{
	public static void MakeTokensExpired(String accessToken, String refreshToken)
	{
		String updateTokensQuery = "UPDATE Tokens SET refresh_expiration = @expiration, access_expiration = @expiration WHERE refresh_token = @refreshToken AND access_token = @accessToken";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(updateTokensQuery, connection);
		command.Parameters.AddWithValue("@expiration", 0);
		command.Parameters.AddWithValue("@refreshToken", refreshToken);
		command.Parameters.AddWithValue("@accessToken", accessToken);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to make tokens expired");
		}
	}

	public static int CreateFamily(String uid)
	{
		String createTokenFamilyQuery = "INSERT INTO TokenFamily(user_id, last_used) OUTPUT INSERTED.id VALUES (@user_id, @last_used)";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(createTokenFamilyQuery, connection);
		command.Parameters.AddWithValue("@last_used", Tools.TimeConverter.DateTimeToUnix(DateTime.Now));
		command.Parameters.AddWithValue("@user_id", uid);
		int familyID;
		try
		{
			familyID = (int)command.ExecuteScalar();
			command.ExecuteNonQuery();
			return familyID;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to create token family");
		}
	}

	internal static TokenSet CreateTokens(User user, int familyID)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String refreshToken = Tools.RandomString.Generate(32);
		String accessToken = Tools.RandomString.Generate(32);
		String createTokenRecord = "INSERT INTO Tokens (family, access_token, refresh_token, access_expiration, refresh_expiration) VALUES (@family_id, @access_token, @refresh_token, @access_expiration, @refresh_expiration)";
		SqlCommand command = new SqlCommand(createTokenRecord, connection);
		command.Parameters.AddWithValue("@family_id", familyID);
		command.Parameters.AddWithValue("@access_token", accessToken);
		command.Parameters.AddWithValue("@refresh_token", refreshToken);
		command.Parameters.AddWithValue("@access_expiration", Authentication.Expiration.GenerateAccess(30));
		command.Parameters.AddWithValue("@refresh_expiration", Authentication.Expiration.GenerateRefresh(30));
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to create token record");
		}
		String updateValidTokenQuery = "UPDATE TokenFamily SET valid_token = @accessToken WHERE id = @familyID";
		command = new SqlCommand(updateValidTokenQuery, connection);
		command.Parameters.AddWithValue("@accessToken", accessToken);
		command.Parameters.AddWithValue("@familyID", familyID);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to update valid token");
		}
		return new TokenSet(refreshToken, accessToken);
	}

	public static Boolean InvalidatedFamilyCorrectly(int familyID)
	{
		String getFamilyQuery = "SELECT * FROM TokenFamily WHERE id = @familyID";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@familyID", familyID);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getFamilyQuery, parameters);
		Boolean correctTokenFamilyDeletion = false;
		try
		{
			if (data == null)
			{
				correctTokenFamilyDeletion = true;
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to get token family");
		}
		String getTokensQuery = "SELECT * FROM Tokens WHERE family = @familyID";
		parameters = new Dictionary<string, object>();
		parameters.Add("@familyID", familyID);
		Dictionary<String, object>? tokens = Data.Database.Reader.ReadOne(getTokensQuery, parameters);
		Boolean correctTokenDeletion = false;
		try
		{
			if (tokens == null)
			{
				correctTokenDeletion = true;
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to get tokens");
		}

		return correctTokenFamilyDeletion && correctTokenDeletion;
	}
}