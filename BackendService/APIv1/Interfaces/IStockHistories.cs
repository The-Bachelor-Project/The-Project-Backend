namespace API.v1.Interfaces;

public interface IStockHistories
{
	public Data.StockProfile[] Get(string ticker, string exchange, string startDate, string endDate, string interval, string accessToken);
}