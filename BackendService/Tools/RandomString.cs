namespace Tools;

public class RandomString
{
	private static readonly Random _random = new Random();
	private static String characters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

	public static string Generate(int length)
	{
		char[] result = new char[length];
		for (int i = 0; i < length; i++)
		{
			result[i] = characters[_random.Next(characters.Length)];
		}
		return new string(result);
	}
}