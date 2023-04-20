using System.Data.SqlClient;

namespace Authentication;

class CreateFamily
{
	public static int Call()
	{
		int unixLastUsed = Tools.TimeConverter.dateTimeToUnix(DateTime.Now);
		String createTokenFamily = "INSERT INTO TokenFamily(last_used) VALUES (@last_used)";
		SqlConnection connection = new Data.Database.Connection().Create();
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
		connection.Close();
		int familyID = GetFamilyID();
		return familyID;
	}

	private static int GetFamilyID()
	{
		String getFamilyID = "SELECT TOP 1 id FROM TokenFamily ORDER BY id DESC";
		SqlConnection connection = new Data.Database.Connection().Create();
		SqlCommand command = new SqlCommand(getFamilyID, connection);
		SqlDataReader reader = command.ExecuteReader();
		if (reader.Read())
		{
			try
			{
				return int.Parse(reader["id"].ToString()!);
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				return -2;
			}
		}
		else
		{
			return -3;
		}

	}
}