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

		Data.StockHistory stockHistory = await new Data.Fetcher.StockFetcher().GetHistory(stock.ticker, stock.exchange, startDate.AddYears(-1), endDate, "daily");
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
		Data.DatePriceOHLC currencyStockPrice = stockHistory.history.First();
		DateOnly currentDate = Tools.TimeConverter.UnixTimeStampToDateOnly(stockTransactions.First().timestamp!.Value);
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
					currencyStockPrice = stockHistory.history[stockIndex];
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
					new Data.Money(currencyStockPrice.openPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currencyStockPrice.highPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currencyStockPrice.lowPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currencyStockPrice.closePrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY)));
			}
			currentDate = currentDate.AddDays(1);
		}

		return new Data.Position(stock.ticker, stock.exchange, valueHistory, dividendHistory);
	}

	public StockPosition UpdateStockTransactions(DateOnly startDate, DateOnly endDate)
	{
		System.Console.WriteLine("UpdateStockTransactions: " + startDate + " " + endDate + " " + stock.ticker + " " + stock.exchange + " " + portfolio.id);
		SqlConnection connection = new Data.Database.Connection().Create();
		String query = "SELECT * FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp <= @endDate";
		//String query = "SELECT * FROM(SELECT TOP 1 * FROM StockTransactions	WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp < @startDate ORDER BY timestamp DESC) AS first_row UNION ALL SELECT * FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp >= @startDate AND timestamp <= @endDate ORDER BY timestamp DESC";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@portfolio", portfolio.id);
		command.Parameters.AddWithValue("@ticker", stock.ticker);
		command.Parameters.AddWithValue("@exchange", stock.exchange);
		command.Parameters.AddWithValue("@startDate", Tools.TimeConverter.dateOnlyToUnix(startDate));
		command.Parameters.AddWithValue("@endDate", Tools.TimeConverter.dateOnlyToUnix(endDate));

		using (SqlDataReader reader = command.ExecuteReader())
		{
			stockTransactions = new List<StockTransaction>();

			int startTimeStamp = Tools.TimeConverter.dateOnlyToUnix(startDate);

			while (reader.Read())
			{
				System.Console.WriteLine("StockTransactions: " + "    " + reader["ticker"].ToString() + "   " + Tools.TimeConverter.UnixTimeStampToDateOnly(Convert.ToInt32(reader["timestamp"])));

				StockTransaction newStockTransaction = new StockTransaction();

				newStockTransaction.id = reader["id"].ToString();
				newStockTransaction.portfolioId = portfolio.id;
				newStockTransaction.ticker = reader["ticker"].ToString();
				newStockTransaction.exchange = reader["exchange"].ToString();
				newStockTransaction.amount = Convert.ToDecimal(reader["amount"]);
				newStockTransaction.amountAdjusted = Convert.ToDecimal(reader["amount_adjusted"]);
				newStockTransaction.amountOwned = Convert.ToDecimal(reader["amount_owned"]);
				newStockTransaction.timestamp = Convert.ToInt32(reader["timestamp"]);
				newStockTransaction.price = new Money(Convert.ToDecimal(reader["price_amount"]), reader["price_currency"].ToString()!);

				if (newStockTransaction.timestamp < startTimeStamp && stockTransactions.Count == 1)
				{
					stockTransactions = new List<StockTransaction>();
				}
				stockTransactions.Add(newStockTransaction);
			}
			reader.Close();

			return this;
		}
	}
}