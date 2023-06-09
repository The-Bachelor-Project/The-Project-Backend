namespace Tools;
using System.Text.RegularExpressions;

public class ValidEmail
{
	public static Boolean Check(String email)
	{
		String regexPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
		Regex regex = new Regex(regexPattern);
		return regex.IsMatch(email);
	}
}