namespace BackendService.tests;

public class TransactionHelper
{
	public static List<TransactionTestObject> CombinedTransactions(GetTransactionsResponse list)
	{
		List<TransactionTestObject> transactions = new List<TransactionTestObject>();
		foreach (StockApp.StockTransaction stockTransaction in list.portfolios[0].stockTransactions)
		{
			// transactions.Add(new TransactionTestObject((int)stockTransaction.timestamp!, stockTransaction.balance!.amount));
		}
		foreach (StockApp.CashTransaction cashTransaction in list.portfolios[0].cashTransactions)
		{
			// transactions.Add(new TransactionTestObject((int)cashTransaction.timestamp!, cashTransaction.balance!.amount));
		}
		foreach (StockApp.DividendPayout dividendPayout in list.portfolios[0].dividendPayouts)
		{
			// transactions.Add(new TransactionTestObject((int)dividendPayout.timestamp!, dividendPayout.balance!.amount));
		}
		transactions.Sort((x, y) => x.timestamp.CompareTo(y.timestamp));
		return transactions;
	}
}

public class TransactionTestObject
{
	public int timestamp { get; set; }
	public Decimal balanceAmount { get; set; }
	public TransactionTestObject(int timestamp, Decimal balanceAmount)
	{
		this.timestamp = timestamp;
		this.balanceAmount = balanceAmount;
	}
}