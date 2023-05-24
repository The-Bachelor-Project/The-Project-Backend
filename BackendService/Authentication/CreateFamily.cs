using System.Data.SqlClient;

namespace Authentication;

class CreateFamily
{
	public static int Call(String uid)
	{
		int unixLastUsed = Tools.TimeConverter.dateTimeToUnix(DateTime.Now);
		String createTokenFamily = "INSERT INTO TokenFamily(user_id, last_used) VALUES (@user_id, @last_used)";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(createTokenFamily, connection);
		command.Parameters.AddWithValue("@last_used", unixLastUsed);
		command.Parameters.AddWithValue("@user_id", uid);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			return -1;
		}
		int familyID = GetFamilyID();
		return familyID;
	}

	private static int GetFamilyID()
	{
		String getFamilyID = "SELECT TOP 1 id FROM TokenFamily ORDER BY id DESC";
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getFamilyID);
		if (data != null)
		{
			int familyId = int.Parse(data["id"].ToString()!);
			return familyId;
		}
		return -1;
	}
}