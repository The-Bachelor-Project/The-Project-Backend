namespace Tools;

public class TimeConverter
{
	public static int DateOnlyToUnix(DateOnly dateOnly)
	{
		try
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = dateOnly.ToDateTime(new TimeOnly(12, 0)).ToUniversalTime() - origin;
			return int.Parse(Math.Floor(diff.TotalSeconds).ToString());
		}
		catch (System.Exception)
		{
			throw new StatusCodeException(400, "The date " + dateOnly.ToString() + " is not valid");
		}
	}
	public static DateTime UnixTimeStampToDateTime(int unix)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dateTime = dateTime.AddSeconds(unix).ToUniversalTime();
		return dateTime;
	}
	public static DateOnly UnixTimeStampToDateOnly(int unix)
	{
		return DateOnly.FromDateTime(UnixTimeStampToDateTime(unix));
	}

	public static int DateTimeToUnix(DateTime dateTime)
	{
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan diff = dateTime.ToUniversalTime() - origin;
		return int.Parse(Math.Floor(diff.TotalSeconds).ToString());
	}



	public static String DateOnlyToString(DateOnly dateOnly)
	{
		String res = dateOnly.Year.ToString();
		res += dateOnly.Month >= 10 ? "-" : "-0";
		res += dateOnly.Month;
		res += dateOnly.Day >= 10 ? "-" : "-0";
		res += dateOnly.Day;
		return res;
	}
}