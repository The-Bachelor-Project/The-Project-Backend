namespace Data.Interfaces;

interface IStockHistoryDaily
{
	public Task<Data.StockHistory> Usd(String ticker, String exchange, DateOnly startDate, DateOnly endDate);
}