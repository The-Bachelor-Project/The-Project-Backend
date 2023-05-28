using System.Data.SqlClient;

namespace Authentication;

public class CreateFamily
{
	public static int Call(String uid)
	{
		int unixLastUsed = Tools.TimeConverter.DateTimeToUnix(DateTime.Now);
		String createTokenFamily = "INSERT INTO TokenFamily(user_id, last_used) OUTPUT INSERTED.id VALUES (@user_id, @last_used)";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(createTokenFamily, connection);
		command.Parameters.AddWithValue("@last_used", unixLastUsed);
		command.Parameters.AddWithValue("@user_id", uid);
		try
		{
			int familyID = (int)command.ExecuteScalar();
			command.ExecuteNonQuery();
			return familyID;
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(404, "Token family was not found");
		}
	}
}