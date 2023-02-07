class TimeConverter
{
	public static int dateOnlyToUnix(DateOnly dateOnly)
	{
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan diff = dateOnly.ToDateTime(new TimeOnly(0)).ToUniversalTime() - origin;
		return int.Parse(Math.Floor(diff.TotalSeconds).ToString());
	}
}