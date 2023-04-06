namespace API.v1.Interfaces;

public interface IUsers
{
	public Data.User Get(string uid);
	public Data.User Post(Data.User newUser);
}