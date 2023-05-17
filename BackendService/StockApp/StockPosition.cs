using System.Data.SqlClient;

namespace StockApp;

public class StockPosition
{
	public StockPosition(Portfolio portfolio, Stock stock)
	{
		this.portfolio = portfolio;
		this.stock = stock;
	}
	public Portfolio portfolio { get; set; }
	public Stock stock { get; set; }

	public List<StockTransaction> stockTransactions { get; set; } = new List<StockTransaction>();

	public async Task<Data.Position> GetValueHistory(string currency, DateOnly startDate, DateOnly endDate)
	{
		UpdateStockTransactions(startDate.AddYears(-1), endDate);
		List<Data.DatePriceOHLC> valueHistory = new List<Data.DatePriceOHLC>();
		List<Data.Dividend> dividendHistory = new List<Data.Dividend>();

		decimal currentlyOwned = 0;
		System.Console.WriteLine("Stock ffd   " + stock.ticker + "    " + stockTransactions.Count);

		if (stockTransactions.Count == 0)
		{
			return new Data.Position(stock.ticker, stock.exchange, valueHistory, dividendHistory);
		}


		if (Tools.TimeConverter.UnixTimeStampToDateOnly(stockTransactions.First().timestamp!.Value) < startDate)
		{
			currentlyOwned = stockTransactions.First().amountOwned!.Value;
		}
		/* Should still work with out this code since it will always result in currentlyOwned = 0, which was already the case
		else
		{
			currentlyOwned = stockTransactions.First().amountOwned!.Value - stockTransactions.First().amountAdjusted!.Value;
		}
		*/

		DateOnly currentDate = Tools.TimeConverter.UnixTimeStampToDateOnly(stockTransactions.First().timestamp!.Value);
		currentDate = currentDate < startDate ? currentDate : startDate;
		Data.StockHistory stockHistory = await new Data.Fetcher.StockFetcher().GetHistory(stock.ticker, stock.exchange, currentDate, endDate, "daily");
		Data.DatePriceOHLC currentStockPrice = stockHistory.history.First();

		int dividendIndex = 0;
		Data.Dividend? dividend = null;
		if (stockHistory.dividends.Count != 0)
		{
			dividendIndex = stockHistory.dividends.FindIndex(divi => divi.date >= currentDate);
			dividend = stockHistory.dividends[dividendIndex];
		}
		int transactionIndex = 0;
		int stockIndex = 0;


		while (currentDate <= endDate)
		{

			if (transactionIndex < stockTransactions.Count)
			{
				System.Console.WriteLine(currentDate);
				while (Tools.TimeConverter.UnixTimeStampToDateOnly(stockTransactions[transactionIndex].timestamp!.Value) == currentDate)
				{
					currentlyOwned = stockTransactions[transactionIndex].amountOwned!.Value;
					transactionIndex++;
					if (transactionIndex >= stockTransactions.Count)
					{
						break;
					}
				}
			}
			if (stockIndex < stockHistory.history.Count)
			{
				if (stockHistory.history[stockIndex].date == currentDate)
				{
					currentStockPrice = stockHistory.history[stockIndex];
					stockIndex++;
				}
			}
			if (dividendIndex < stockHistory.dividends.Count && dividend != null)
			{
				if (currentDate == stockHistory.dividends[dividendIndex].date)
				{
					dividend = stockHistory.dividends[dividendIndex];
					dividendIndex++;

					dividendHistory.Add(new Data.Dividend(currentDate, new Data.Money(dividend.payout.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY)));
				}
			}
			if (currentDate >= startDate)
			{
				valueHistory.Add(new Data.DatePriceOHLC(currentDate,
					new Data.Money(currentStockPrice.openPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currentStockPrice.highPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currentStockPrice.lowPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currentStockPrice.closePrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY)));
			}
			currentDate = currentDate.AddDays(1);
		}

		return new Data.Position(stock.ticker, stock.exchange, valueHistory, dividendHistory);
	}

	public StockPosition UpdateStockTransactions(DateOnly startDate, DateOnly endDate)
	{
		System.Console.WriteLine("UpdateStockTransactions: " + startDate + " " + endDate + " " + stock.ticker + " " + stock.exchange + " " + portfolio.id);
		String query = "SELECT * FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp <= @endDate";
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@portfolio", portfolio.id);
		parameters.Add("@ticker", stock.ticker);
		parameters.Add("@exchange", stock.exchange);
		parameters.Add("@startDate", Tools.TimeConverter.dateOnlyToUnix(startDate));
		parameters.Add("@endDate", Tools.TimeConverter.dateOnlyToUnix(endDate));
		List<Dictionary<String, object>> data = Data.Database.Reader.ReadData(query, parameters);
		//String query = "SELECT * FROM(SELECT TOP 1 * FROM StockTransactions	WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp < @startDate ORDER BY timestamp DESC) AS first_row UNION ALL SELECT * FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp >= @startDate AND timestamp <= @endDate ORDER BY timestamp DESC";

		stockTransactions = new List<StockTransaction>();

		int startTimeStamp = Tools.TimeConverter.dateOnlyToUnix(startDate);

		foreach (Dictionary<String, object> row in data)
		{
			System.Console.WriteLine("StockTransactions: " + "    " + row["ticker"].ToString() + "   " + Tools.TimeConverter.UnixTimeStampToDateOnly(Convert.ToInt32(row["timestamp"])));

			StockTransaction newStockTransaction = new StockTransaction();

			newStockTransaction.id = int.Parse(row["id"].ToString()!);
			newStockTransaction.portfolioId = portfolio.id;
			newStockTransaction.ticker = row["ticker"].ToString();
			newStockTransaction.exchange = row["exchange"].ToString();
			newStockTransaction.amount = Convert.ToDecimal(row["amount"]);
			newStockTransaction.amountAdjusted = Convert.ToDecimal(row["amount_adjusted"]);
			newStockTransaction.amountOwned = Convert.ToDecimal(row["amount_owned"]);
			newStockTransaction.timestamp = Convert.ToInt32(row["timestamp"]);
			newStockTransaction.price = new Money(Convert.ToDecimal(row["price_amount"]), row["price_currency"].ToString()!);

			if (newStockTransaction.timestamp < startTimeStamp && stockTransactions.Count == 1)
			{
				stockTransactions = new List<StockTransaction>();
			}
			stockTransactions.Add(newStockTransaction);
		}

		return this;
	}
}