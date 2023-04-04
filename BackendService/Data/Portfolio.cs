namespace Data;
public class Portfolio
{
	public Portfolio(string name, string owner, string currency, decimal balance, bool trackBalance)
	{
		this.Name = name;
		this.Owner = owner;
		this.Currency = currency;
		this.Balance = balance;
		this.TrackBalance = trackBalance;
	}

	public String? UID { get; set; }
	public String Name { get; set; }
	public String Owner { get; set; }
	public String Currency { get; set; }
	public Decimal Balance { get; set; }
	public Boolean TrackBalance { get; set; }
}