namespace API.v1.Interfaces;

public interface IUsers
{
	public Data.User Get(string uid);
	public bool Post(Data.User newUser);
}