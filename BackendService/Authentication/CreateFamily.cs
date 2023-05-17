using System.Data.SqlClient;

namespace Authentication;

class CreateFamily
{
	public static int Call()
	{
		int unixLastUsed = Tools.TimeConverter.dateTimeToUnix(DateTime.Now);
		String createTokenFamily = "INSERT INTO TokenFamily(last_used) VALUES (@last_used)";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(createTokenFamily, connection);
		command.Parameters.AddWithValue("@last_used", unixLastUsed);
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
		return -3;
	}
}