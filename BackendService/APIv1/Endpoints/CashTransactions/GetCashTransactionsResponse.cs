namespace API.v1;

public class GetCashTransactionsResponse
{
	public String response { get; set; }
	public List<StockApp.CashTransaction> cashTransactions { get; set; }

	public GetCashTransactionsResponse(String response, List<StockApp.CashTransaction> cashTransactions)
	{
		this.response = response;
		this.cashTransactions = cashTransactions;
	}
}
