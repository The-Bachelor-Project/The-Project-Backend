namespace API.v1.Interfaces;

public interface IStockProfiles
{
	public Data.StockProfile[] Get(string ticker, string exchange);
}