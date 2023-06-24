namespace API.v1;

public class PutPortfoliosBody
{
	public PutPortfoliosBody(String newCurrency, String newName, String id)
	{
		this.newCurrency = newCurrency;
		this.newName = newName;
		this.id = id;
	}
	public String newCurrency { get; set; }
	public String newName { get; set; }
	public String id { get; set; }
}
