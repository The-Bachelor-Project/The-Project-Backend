namespace API.v1;

public class PostPortfoliosBody
{
	public PortfolioBody portfolio { get; }

	public PostPortfoliosBody(PortfolioBody portfolio)
	{
		this.portfolio = portfolio;
	}
}

public class PortfolioBody
{
	public PortfolioBody(string name, string currency)
	{
		this.name = name;
		this.currency = currency;
	}
	public String name { get; set; }
	public String currency { get; set; }
}