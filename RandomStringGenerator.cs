public static class RandomStringGenerator
{
    private static readonly Random _random = new Random();

    public static string Generate(int length, string characters)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = characters[_random.Next(characters.Length)];
        }
        return new string(result);
    }
}