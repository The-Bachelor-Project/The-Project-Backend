using System.Data.SqlClient;
namespace Authentication;

class AuthenticateToken
{
	public static Boolean RefreshToken(String RefreshToken)
	{
		String GetIsRefreshExpired = "SELECT dbo.CheckRefreshIsExpired(@refresh_token) AS IsExpired";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(GetIsRefreshExpired, Connection);
		Command.Parameters.AddWithValue("@refresh_token", RefreshToken);
		try
		{
			SqlDataReader Reader = Command.ExecuteReader();
			if (Reader.Read())
			{
				Boolean IsExpired = Reader.GetBoolean(Reader.GetOrdinal("IsExpired"));
				Reader.Close();
				return IsExpired;
			}
			else
			{
				return true;
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			return true;
		}
	}
}