namespace Data.Fetcher.Interfaces;

interface IStockFetcher
{
	public Task<Data.StockProfile> GetProfile(String ticker, String exchange);

	public Task<Data.StockHistory> GetHistory(String ticker, String exchange, DateOnly startDate, DateOnly endDate, String interval, String currency);

	public Task<Data.StockProfile[]> Search(String query);
}