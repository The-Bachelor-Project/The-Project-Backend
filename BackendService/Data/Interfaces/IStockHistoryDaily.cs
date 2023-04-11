namespace Data.Interfaces;

interface IStockHistoryDaily{
	public Task<Data.StockHistory> usd(String ticker, String exchange, DateOnly startDate, DateOnly endDate);
}