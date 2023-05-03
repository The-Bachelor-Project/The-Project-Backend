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

	public async Task<Data.Position> GetValueHistory(string currency, DateOnly startData, DateOnly endDate)
	{
		UpdateStockTransactions(startData, endDate);
		List<Data.DatePrice> valueHistory = new List<Data.DatePrice>();

		Data.StockHistory stockHistory = await new Data.Fetcher.StockFetcher().GetHistory(stock.ticker, stock.exchange, startData, endDate, "daily");
		decimal currentlyOwned = 0;

		if (stockTransactions.Count > 0)
		{
			currentlyOwned = stockTransactions.First().amountOwned!.Value - stockTransactions.First().amountAdjusted!.Value;
		}

		Data.DatePrice currencyStockPrice = stockHistory.history.First();
		DateOnly currentDate = startData;

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

			if (valueHistory.Count > 0 || currentlyOwned != 0)
			{
				valueHistory.Add(new Data.DatePrice(currentDate,
					new Data.Money(currencyStockPrice.openPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currencyStockPrice.highPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currencyStockPrice.lowPrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY),
					new Data.Money(currencyStockPrice.closePrice!.amount * currentlyOwned, Data.Money.DEFAULT_CURRENCY)));
			}
			if (transactionIndex == stockTransactions.Count && currentlyOwned == 0)
			{
				break; //TODO check if this works
			}
			currentDate = currentDate.AddDays(1);
		}

		return new Data.Position(stock.ticker, stock.exchange, valueHistory);
	}

	public StockPosition UpdateStockTransactions(DateOnly startDate, DateOnly endDate)
	{
		SqlConnection connection = new Data.Database.Connection().Create();
		// String query = "SELECT * FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp >= @startDate AND timestamp <= @endDate";
		String query = "SELECT * FROM(SELECT TOP 1 * FROM StockTransactions	WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp < @startDate ORDER BY timestamp DESC) AS last_row UNION ALL SELECT * FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp >= @startDate AND timestamp <= @endDate";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@portfolio", portfolio.id);
		command.Parameters.AddWithValue("@ticker", stock.ticker);
		command.Parameters.AddWithValue("@exchange", stock.exchange);
		command.Parameters.AddWithValue("@startDate", Tools.TimeConverter.dateOnlyToUnix(startDate));
		command.Parameters.AddWithValue("@endDate", Tools.TimeConverter.dateOnlyToUnix(endDate));

		using (SqlDataReader reader = command.ExecuteReader())
		{
			stockTransactions = new List<StockTransaction>();
			System.Console.WriteLine("StockTransactions: " + reader.HasRows);

			while (reader.Read())
			{
				System.Console.WriteLine("StockTransactions: " + Tools.TimeConverter.UnixTimeStampToDateOnly(Convert.ToInt32(reader["timestamp"])));
				stockTransactions.Add(new StockTransaction());
				stockTransactions.Last().id = reader["id"].ToString();
				stockTransactions.Last().portfolioId = portfolio.id;
				stockTransactions.Last().ticker = reader["ticker"].ToString();
				stockTransactions.Last().exchange = reader["exchange"].ToString();
				stockTransactions.Last().amount = Convert.ToDecimal(reader["amount"]);
				stockTransactions.Last().amountAdjusted = Convert.ToDecimal(reader["amount_adjusted"]);
				stockTransactions.Last().amountOwned = Convert.ToDecimal(reader["amount_owned"]);
				stockTransactions.Last().timestamp = Convert.ToInt32(reader["timestamp"]);
				stockTransactions.Last().price = new Money(Convert.ToDecimal(reader["price_amount"]), reader["price_currency"].ToString()!);
			}
			reader.Close();
			return this;
		}
	}
}