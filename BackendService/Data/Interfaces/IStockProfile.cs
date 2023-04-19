namespace Data.Interfaces;

interface IStockProfile
{
	public Task<Data.StockProfile> Get(String ticker, String exchange);

	public Task<Data.StockProfile[]> Search(String query);
}