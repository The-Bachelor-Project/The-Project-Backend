namespace API.v1;

public class PutPortfoliosBody
{
	public PutPortfoliosBody(String changeString, String id)
	{
		this.changeString = changeString;
		this.id = id;
	}
	public String changeString { get; set; }
	public String id { get; set; }
}
