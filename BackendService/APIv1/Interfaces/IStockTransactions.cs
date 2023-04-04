namespace API.v1.Interfaces;

public interface IStockTransactions
{
	public Data.StockTransaction[] Get(string user, string id, string ticker, string exchange, string portfolio);
	public bool Post(Data.StockTransaction stockTransaction);
}