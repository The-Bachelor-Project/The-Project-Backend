class TimeConverter
{
	public static int dateOnlyToUnix(DateOnly dateOnly)
	{
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan diff = dateOnly.ToDateTime(new TimeOnly(12, 0)).ToUniversalTime() - origin;
		return int.Parse(Math.Floor(diff.TotalSeconds).ToString());
	}
	public static String dateOnlyToString(DateOnly dateOnly)
	{
		String res = dateOnly.Year.ToString();
		res += dateOnly.Month >= 10 ? "-" : "-0";
		res += dateOnly.Month;
		res += dateOnly.Day >= 10 ? "-" : "-0";
		res += dateOnly.Day;
		return res;
	}
}