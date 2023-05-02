using System.Security.Cryptography;
using System.Text;
using Data;

namespace Tools;

public class PriceHistoryConverter
{
	public async Task<List<Data.DatePrice>> Convert(List<Data.DatePrice> priceHistory, String newCurrency)
	{
		if (priceHistory.Count == 0)
		{
			return priceHistory;
		}
		newCurrency = newCurrency.ToUpper();
		if (newCurrency != "USD")
		{
			throw new Exception("Only USD is supported at the moment");
		}
		if (priceHistory.First().closePrice.currency.ToUpper() == "USD")
		{
			return priceHistory;
		}

		DateOnly startDate = priceHistory.First().date;
		DateOnly endDate = priceHistory.Last().date;
		Data.CurrencyHistory currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(priceHistory.First().closePrice.currency, startDate, endDate);

		Dictionary<DateOnly, Data.DatePrice> priceDictionary = priceHistory.ToDictionary(x => x.date, x => x);
		Dictionary<DateOnly, Data.DatePrice> currencyDictionary = currencyHistory.history.ToDictionary(x => x.date, x => x);
		List<Data.DatePrice> newPriceHistory = new List<Data.DatePrice>();
		foreach (DateOnly date in priceDictionary.Keys.Intersect(currencyDictionary.Keys))
		{
			Data.DatePrice currencyPrice = currencyDictionary[date];
			priceDictionary[date].openPrice.amount *= currencyPrice.openPrice.amount;
			priceDictionary[date].closePrice.amount *= currencyPrice.closePrice.amount;
			priceDictionary[date].highPrice.amount *= currencyPrice.highPrice.amount;
			priceDictionary[date].lowPrice.amount *= currencyPrice.lowPrice.amount;

			priceDictionary[date].openPrice.currency = newCurrency;
			priceDictionary[date].closePrice.currency = newCurrency;
			priceDictionary[date].highPrice.currency = newCurrency;
			priceDictionary[date].lowPrice.currency = newCurrency;

			newPriceHistory.Add(priceDictionary[date]);
		}

		priceHistory = newPriceHistory;
		return priceHistory;
	}
}