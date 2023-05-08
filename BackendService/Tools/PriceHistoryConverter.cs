using System.Security.Cryptography;
using System.Text;
using Data;

namespace Tools;

public class PriceHistoryConverter
{
	public async Task<List<Data.DatePriceOHLC>> ConvertStockPrice(List<Data.DatePriceOHLC> priceHistory, String newCurrency)
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

		Dictionary<DateOnly, Data.DatePriceOHLC> priceDictionary = priceHistory.ToDictionary(x => x.date, x => x);
		Dictionary<DateOnly, Data.DatePriceOHLC> currencyDictionary = currencyHistory.history.ToDictionary(x => x.date, x => x);
		List<Data.DatePriceOHLC> newPriceHistory = new List<Data.DatePriceOHLC>();
		foreach (DateOnly date in priceDictionary.Keys.Intersect(currencyDictionary.Keys))
		{
			Data.DatePriceOHLC currencyPrice = currencyDictionary[date];
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

	public async Task<List<Data.Dividend>> ConvertStockDividends(List<Dividend> dividends, String newCurrency)
	{
		if (dividends.Count == 0)
		{
			return dividends;
		}
		newCurrency = newCurrency.ToUpper();
		if (newCurrency != "USD")
		{
			throw new Exception("Only USD is supported at the moment");
		}
		if (dividends.First().payout.currency.ToUpper() == "USD")
		{
			return dividends;
		}

		DateOnly startDate = dividends.First().date;
		DateOnly endDate = dividends.Last().date;
		Data.CurrencyHistory currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(dividends.First().payout.currency, startDate, endDate);

		Dictionary<DateOnly, Data.Dividend> dividendDictionary = dividends.ToDictionary(x => x.date, x => x);
		Dictionary<DateOnly, Data.DatePriceOHLC> currencyDictionary = currencyHistory.history.ToDictionary(x => x.date, x => x);
		List<Data.Dividend> newDividendHistory = new List<Data.Dividend>();
		foreach (DateOnly date in dividendDictionary.Keys.Intersect(currencyDictionary.Keys))
		{
			Data.DatePriceOHLC currencyPrice = currencyDictionary[date];
			dividendDictionary[date].payout.amount *= currencyPrice.closePrice.amount;

			dividendDictionary[date].payout.currency = newCurrency;

			newDividendHistory.Add(dividendDictionary[date]);
		}

		dividends = newDividendHistory;
		return dividends;
	}
}