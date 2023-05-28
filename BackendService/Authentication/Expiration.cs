namespace Authentication;


public class Expiration
{
	public static int GenerateRefresh(int expirationInHours)
	{
		return Tools.TimeConverter.DateTimeToUnix(DateTime.Now.AddHours(expirationInHours));
	}

	public static int GenerateAccess(int expirationInMinutes)
	{
		return Tools.TimeConverter.DateTimeToUnix(DateTime.Now.AddMinutes(expirationInMinutes));
	}

}
