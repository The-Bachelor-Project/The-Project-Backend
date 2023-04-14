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
	public PortfolioBody(string name, string owner, string currency, decimal balance, bool trackBalance)
	{
		this.Name = name;
		this.Owner = owner;
		this.Currency = currency;
		this.Balance = balance;
		this.TrackBalance = trackBalance;
	}
	public String Name { get; set; }
	public String Owner { get; set; }
	public String Currency { get; set; }
	public Decimal Balance { get; set; }
	public Boolean TrackBalance { get; set; }
}