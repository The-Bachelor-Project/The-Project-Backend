namespace Data.Interfaces;

interface ICurrencyHistory
{
	public Task<Data.CurrencyHistory> Usd(String currency, DateOnly startDate, DateOnly endDate);
}