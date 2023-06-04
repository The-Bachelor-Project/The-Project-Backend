namespace BackendService.tests;
using System.Data.SqlClient;
public class CashTransactionHelper
{
	public static StockApp.CashTransaction Get(int id)
	{
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String getTransactionQuery = "SELECT * FROM CashTransactions WHERE id = @id";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@id", id);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getTransactionQuery, parameters);
		if (data == null)
		{
			throw new StatusCodeException(404, "Cash transaction not found");
		}
		StockApp.CashTransaction cashTransaction = new StockApp.CashTransaction();
		cashTransaction.id = (int)data["id"];
		cashTransaction.portfolioId = (String)data["portfolio"];
		cashTransaction.nativeAmount = new StockApp.Money((Decimal)data["native_amount"], (String)data["currency"]);
		cashTransaction.usdAmount = new StockApp.Money((Decimal)data["amount"], "USD");
		cashTransaction.timestamp = (int)data["timestamp"];
		cashTransaction.description = (String)data["description"];
		return cashTransaction;
	}
}