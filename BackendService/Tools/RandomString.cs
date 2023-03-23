namespace Tools;

public class RandomString
{
	private static readonly Random _Random = new Random();
	private static String Characters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

	public static string Generate(int length)
	{
		char[] result = new char[length];
		for (int i = 0; i < length; i++)
		{
			result[i] = Characters[_Random.Next(Characters.Length)];
		}
		return new string(result);
	}
}