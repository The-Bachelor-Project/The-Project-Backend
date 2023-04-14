using System.Data.SqlClient;

namespace Authentication;

class CreateFamily
{
	public static int call()
	{
		int UnixLastUsed = Tools.TimeConverter.dateTimeToUnix(DateTime.Now);
		String CreateTokenFamily = "INSERT INTO TokenFamily(last_used) VALUES (@last_used)";
		SqlConnection Connection = new Data.Database.Connection().Create();
		SqlCommand Command = new SqlCommand(CreateTokenFamily, Connection);
		Command.Parameters.AddWithValue("@last_used", UnixLastUsed);
		try
		{
			Command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			return -1;
		}
		Connection.Close();
		int FamilyID = GetFamilyID();
		return FamilyID;
	}

	private static int GetFamilyID()
	{
		String GetFamilyID = "SELECT TOP 1 id FROM TokenFamily ORDER BY id DESC";
		SqlConnection Connection = new Data.Database.Connection().Create();
		SqlCommand Command = new SqlCommand(GetFamilyID, Connection);
		SqlDataReader Reader = Command.ExecuteReader();
		if (Reader.Read())
		{
			try
			{
				return int.Parse(Reader["id"].ToString()!);
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