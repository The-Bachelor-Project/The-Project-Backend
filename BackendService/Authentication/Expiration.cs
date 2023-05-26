namespace Authentication;


public class Expiration
{
	public static int GenerateRefresh(int expirationInHours)
	{
		return Tools.TimeConverter.dateTimeToUnix(DateTime.Now.AddHours(expirationInHours));
	}

	public static int GenerateAccess(int expirationInMinutes)
	{
		return Tools.TimeConverter.dateTimeToUnix(DateTime.Now.AddMinutes(expirationInMinutes));
	}

}
