using System.Security.Cryptography;
using System.Text;
using Data;

namespace Tools;

public class StockHistoryConverter
{
	public async Task<Data.StockHistory> Convert(Data.StockHistory stockHistory, String newCurrency)
	{
		newCurrency = newCurrency.ToUpper();
		if (newCurrency != "USD")
		{
			throw new Exception("Only USD is supported at the moment");
		}
		if (stockHistory.history.First().closePrice.currency.ToUpper() == "USD")
		{
			return stockHistory;
		}

		DateOnly startDate = stockHistory.startDate!.Value;
		DateOnly endDate = stockHistory.endDate!.Value;

		Data.CurrencyHistory currencyHistory = await new Data.Fetcher.CurrencyFetcher().GetHistory(stockHistory.history.First().closePrice.currency, startDate, endDate);

		int currencyCounter = 0;

		foreach (Data.DatePrice datePrice in stockHistory.history)
		{
			bool found = false;

			while (!found)
			{
				if (currencyCounter >= currencyHistory.history.Count)
				{
					throw new Exception("Currency history is shorter than stock history");
				}
				if (currencyHistory.history[currencyCounter].date == datePrice.date)
				{
					found = true;
					datePrice.openPrice.amount *= currencyHistory.history[currencyCounter].openPrice.amount;
					datePrice.highPrice.amount *= currencyHistory.history[currencyCounter].highPrice.amount;
					datePrice.lowPrice.amount *= currencyHistory.history[currencyCounter].lowPrice.amount;
					datePrice.closePrice.amount *= currencyHistory.history[currencyCounter].closePrice.amount;

					datePrice.openPrice.currency = newCurrency;
					datePrice.highPrice.currency = newCurrency;
					datePrice.lowPrice.currency = newCurrency;
					datePrice.closePrice.currency = newCurrency;
				}
				currencyCounter++;
			}
		}

		return stockHistory;
	}
}