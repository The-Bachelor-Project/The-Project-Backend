namespace API.v1;

class PostPortfoliosBody
{
	public PortfolioBody portfolio { get; }
	public string accessToken { get; }

	public PostPortfoliosBody(PortfolioBody portfolio, string accessToken)
	{
		this.portfolio = portfolio;
		this.accessToken = accessToken;
	}
}

class PortfolioBody
{
	public PortfolioBody(string name, string currency, decimal balance, bool trackBalance)
	{
		this.name = name;
		this.currency = currency;
		this.balance = balance;
		this.trackBalance = trackBalance;
	}
	public String name { get; set; }
	public String currency { get; set; }
	public Decimal balance { get; set; }
	public Boolean trackBalance { get; set; }
}