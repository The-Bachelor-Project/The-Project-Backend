namespace Data;

public class Transaction
{
	public Transaction(string type, string portfolio, int timestamp, string description, StockApp.Money value, StockApp.Money balance, StockApp.Money combinedBalance)
	{
		this.type = type;
		this.portfolio = portfolio;
		this.timestamp = timestamp;
		this.description = description;
		this.value = value;
		this.balance = balance;
		this.combinedBalance = combinedBalance;
	}

	public String type { get; set; }
	public String portfolio { get; set; }
	public int timestamp { get; set; }
	public String description { get; set; }
	public StockApp.Money value { get; set; }
	public StockApp.Money balance { get; set; }
	public StockApp.Money combinedBalance { get; set; }

}