namespace Data.Fetcher.Interfaces;

interface ICurrencyFetcher
{
	public Task<Data.CurrencyHistory> GetHistory(String currency, DateOnly startDate, DateOnly endDate);
}