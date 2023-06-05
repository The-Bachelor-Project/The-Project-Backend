using System.Security.Cryptography;
using System.Text;
using Data;

namespace Tools;

public class PriceConverter
{
	public async Task<List<Data.DatePriceOHLC>> ConvertStockPrice(List<Data.DatePriceOHLC> priceHistory, String newCurrency, Boolean convertFromUSDToNewCurrency)
	{
		if (priceHistory.Count == 0)
		{
			return priceHistory;
		}
		newCurrency = newCurrency.ToUpper();
		if (priceHistory.First().closePrice.currency.ToUpper() == "USD" && !convertFromUSDToNewCurrency)
		{
			return priceHistory;
		}
		if (convertFromUSDToNewCurrency && newCurrency == "USD")
		{
			return priceHistory;
		}

		DateOnly startDate = priceHistory.First().date;
		DateOnly endDate = priceHistory.Last().date;
		Data.CurrencyHistory currencyHistory;
		if (convertFromUSDToNewCurrency)
		{
			currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(newCurrency, startDate, endDate);
		}
		else
		{
			currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(priceHistory.First().closePrice.currency.ToUpper(), startDate, endDate);
		}

		if (currencyHistory.history.Count == 0)
		{
			throw new StatusCodeException(500, "Currency exchange rate list of " + priceHistory.First().closePrice.currency + " is empty");
		}
		Dictionary<DateOnly, Data.DatePriceOHLC> priceDictionary = priceHistory.ToDictionary(x => x.date, x => x);
		Dictionary<DateOnly, Data.DatePriceOHLC> currencyDictionary = currencyHistory.history.ToDictionary(x => x.date, x => x);
		List<Data.DatePriceOHLC> newPriceHistory = new List<Data.DatePriceOHLC>();
		if (convertFromUSDToNewCurrency)
		{
			foreach (DateOnly date in priceDictionary.Keys.Intersect(currencyDictionary.Keys))
			{
				Data.DatePriceOHLC currencyPrice = currencyDictionary[date];
				priceDictionary[date].openPrice.amount *= (1 / currencyPrice.openPrice.amount);
				priceDictionary[date].closePrice.amount *= (1 / currencyPrice.closePrice.amount);
				priceDictionary[date].highPrice.amount *= (1 / currencyPrice.highPrice.amount);
				priceDictionary[date].lowPrice.amount *= (1 / currencyPrice.lowPrice.amount);

				priceDictionary[date].openPrice.currency = newCurrency;
				priceDictionary[date].closePrice.currency = newCurrency;
				priceDictionary[date].highPrice.currency = newCurrency;
				priceDictionary[date].lowPrice.currency = newCurrency;

				newPriceHistory.Add(priceDictionary[date]);
			}
		}
		else
		{
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
		}


		priceHistory = newPriceHistory;
		return priceHistory;
	}

	public async Task<List<Data.Dividend>> ConvertStockDividends(List<Dividend> dividends, String newCurrency, Boolean convertFromUSDToNewCurrency)
	{
		if (dividends.Count == 0)
		{
			return dividends;
		}
		newCurrency = newCurrency.ToUpper();
		if (dividends.First().payout.currency == "USD" && !convertFromUSDToNewCurrency)
		{
			return dividends;
		}
		if (convertFromUSDToNewCurrency && newCurrency == "USD")
		{
			return dividends;
		}

		DateOnly startDate = dividends.First().date;
		DateOnly endDate = dividends.Last().date;
		Data.CurrencyHistory currencyHistory;
		if (convertFromUSDToNewCurrency)
		{
			currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(newCurrency, startDate, endDate);
		}
		else
		{
			currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(dividends.First().payout.currency, startDate, endDate);
		}
		if (currencyHistory.history.Count == 0)
		{
			throw new StatusCodeException(500, "Currency exchange rate list of " + newCurrency + " is empty");
		}

		Dictionary<DateOnly, Data.Dividend> dividendDictionary = dividends.ToDictionary(x => x.date, x => x);
		Dictionary<DateOnly, Data.DatePriceOHLC> currencyDictionary = currencyHistory.history.ToDictionary(x => x.date, x => x);
		List<Data.Dividend> newDividendHistory = new List<Data.Dividend>();
		if (convertFromUSDToNewCurrency)
		{
			foreach (DateOnly date in dividendDictionary.Keys.Intersect(currencyDictionary.Keys))
			{
				Data.DatePriceOHLC currencyPrice = currencyDictionary[date];
				dividendDictionary[date].payout.amount *= (1 / currencyPrice.closePrice.amount);
				dividendDictionary[date].payout.currency = newCurrency;
				newDividendHistory.Add(dividendDictionary[date]);
			}
		}
		else
		{
			foreach (DateOnly date in dividendDictionary.Keys.Intersect(currencyDictionary.Keys))
			{
				Data.DatePriceOHLC currencyPrice = currencyDictionary[date];
				dividendDictionary[date].payout.amount *= currencyPrice.closePrice.amount;
				dividendDictionary[date].payout.currency = newCurrency;
				newDividendHistory.Add(dividendDictionary[date]);
			}
		}


		dividends = newDividendHistory;
		return dividends;
	}

	public static async Task<List<Decimal>> GetExchangeHistory(List<int> timestamps, String newCurrency)
	{
		if (!(Tools.ValidCurrency.Check(newCurrency)))
		{
			throw new StatusCodeException(400, "Invalid currency");
		}
		if (timestamps.Count == 0)
		{
			return new List<Decimal>();
		}
		if (newCurrency == "USD")
		{
			List<Decimal> temp = Enumerable.Repeat((decimal)1, timestamps.Count).ToList();
			return temp;
		}
		int highestTimestamp = timestamps.Max();
		int lowestTimestamp = timestamps.Min();
		Data.CurrencyHistory currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(newCurrency, Tools.TimeConverter.UnixTimeStampToDateOnly(lowestTimestamp).AddDays(-7), Tools.TimeConverter.UnixTimeStampToDateOnly(highestTimestamp).AddDays(7));
		if (currencyHistory.history.Count == 0)
		{
			throw new StatusCodeException(500, "Currency exchange rate list of " + newCurrency + " is empty");
		}
		Dictionary<DateOnly, Data.DatePriceOHLC> currencyDictionary = currencyHistory.history.ToDictionary(x => x.date, x => x);
		List<Decimal> exchangeRates = new List<Decimal>();
		foreach (int timestamp in timestamps)
		{
			DateOnly date = Tools.TimeConverter.UnixTimeStampToDateOnly(timestamp);
			if (currencyDictionary.ContainsKey(date))
			{
				exchangeRates.Add(currencyDictionary[date].closePrice.amount);
			}
			else
			{
				throw new StatusCodeException(500, "Currency exchange rate list of " + newCurrency + " does not contain " + date.ToString());
			}
		}
		return exchangeRates;
	}

	public static async Task<StockApp.Money> ConvertMoney(StockApp.Money nativeAmount, int timestamp, String newCurrency, Boolean convertFromUSDToNewCurrency)
	{
		if (!(Tools.ValidCurrency.Check(newCurrency)))
		{
			throw new StatusCodeException(400, "Invalid currency");
		}
		if (nativeAmount.amount == 0)
		{
			return new StockApp.Money(0, "USD");
		}
		if (nativeAmount.currency.ToUpper() == "USD" && !convertFromUSDToNewCurrency)
		{
			return nativeAmount;
		}
		newCurrency = newCurrency.ToUpper();
		if (convertFromUSDToNewCurrency && newCurrency == "USD")
		{
			return nativeAmount;
		}

		Data.CurrencyHistory currencyHistory;
		if (convertFromUSDToNewCurrency)
		{
			currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(newCurrency, Tools.TimeConverter.UnixTimeStampToDateOnly((int)timestamp!).AddDays(-7), Tools.TimeConverter.UnixTimeStampToDateOnly((int)timestamp!));
		}
		else
		{
			currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(nativeAmount.currency, Tools.TimeConverter.UnixTimeStampToDateOnly((int)timestamp!).AddDays(-7), Tools.TimeConverter.UnixTimeStampToDateOnly((int)timestamp!));
		}

		if (currencyHistory.history.Count == 0)
		{
			throw new StatusCodeException(500, "Currency exchange rate list of " + newCurrency + " is empty");
		}

		// Find the most recent exchange rate, closest to the timestamp
		DateOnly date = Tools.TimeConverter.UnixTimeStampToDateOnly((int)timestamp!);
		for (int i = currencyHistory.history.Count - 1; i >= 0; i--)
		{
			if (currencyHistory.history[i].date <= date)
			{
				if (convertFromUSDToNewCurrency)
				{
					return new StockApp.Money(nativeAmount.amount * (1 / currencyHistory.history[i].closePrice.amount), newCurrency);
				}
				else
				{
					return new StockApp.Money(nativeAmount.amount * currencyHistory.history[i].closePrice.amount, newCurrency);
				}
			}
		}
		throw new StatusCodeException(500, "Currency exchange rate list of " + newCurrency + " is empty");
	}
}