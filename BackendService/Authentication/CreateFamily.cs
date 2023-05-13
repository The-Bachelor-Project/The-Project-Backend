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
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(getFamilyID, connection);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				try
				{
					int familyId = int.Parse(reader["id"].ToString()!);
					reader.Close();
					return familyId;
				}
				catch (Exception e)
				{
					reader.Close();
					System.Console.WriteLine(e);
					return -2;
				}
			}
			else
			{
				reader.Close();
				return -3;
			}
		}
	}
}