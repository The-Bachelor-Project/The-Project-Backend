namespace API.v1.Interfaces;

public interface ITokens
{
	public Data.TokenSet Get(string refreshToken);
	public bool Post(string email, string password);

	public string GetUser(string accessToken);
}