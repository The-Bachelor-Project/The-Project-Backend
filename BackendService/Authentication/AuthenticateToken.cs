using System.Data.SqlClient;
namespace Authentication;

class AuthenticateToken
{
	public static Boolean AccessToken(String accessToken)
	{
		String GetIsAccessExpired = "SELECT dbo.CheckAccessIsExpired(@access_token) AS IsExpired";
		SqlConnection Connection = DatabaseService.Database.createConnection();
		SqlCommand Command = new SqlCommand(GetIsAccessExpired, Connection);
		Command.Parameters.AddWithValue("@access_token", accessToken);
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

	public static Boolean RefreshToken(String refreshToken)
	{
		return true;
	}
}