namespace StockApp.EndpointHandler;

public class EndpointHandler
{
	public User user;
	public EndpointHandler(String accessToken)
	{
		user = new TokenSet(accessToken).GetUser();
	}
}