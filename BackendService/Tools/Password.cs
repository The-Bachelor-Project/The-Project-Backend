using System.Security.Cryptography;
using System.Text;

namespace Tools;

public class Password
{
	//A function named Hash that takes a password and returns a hash based on the sha256 algorithm
	public static string Hash(string password, string salt)
	{
		//Create a new instance of the SHA256 class
		SHA256 sha256 = SHA256.Create();
		//Convert the input string to a byte array and compute the hash
		byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
		//Create a new Stringbuilder to collect the bytes
		//and create a string.
		StringBuilder builder = new StringBuilder();
		//Loop through each byte of the hashed data 
		//and format each one as a hexadecimal string.
		for (int i = 0; i < bytes.Length; i++)
		{
			builder.Append(bytes[i].ToString("x2"));
		}
		//Return the hexadecimal string.
		return builder.ToString();
	}
}