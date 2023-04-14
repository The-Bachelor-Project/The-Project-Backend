using System.Data.SqlClient;

namespace BusinessLogic;

public class StockTransaction
{
	public String? Id { get; set; }
	public String? PortfolioId { get; set; }
	public String? Ticker { get; set; }
	public String? Exchange { get; set; }
	public Decimal? Amount { get; set; }
	public int? Timestamp { get; set; }
	public String? Currency { get; set; }
	public Decimal? Price { get; set; }



	public async Task<StockTransaction> AddToDb()
	{
		SqlConnection Connection = new Data.Database.Connection().Create();

		try
		{
			await new Data.Fetcher.StockProfile().Get(Ticker, Exchange);
		}
		catch (Exception e)
		{
			throw new CouldNotGetStockException();
		}

		String InsertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, timestamp, price_amount, price_currency) VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @timestamp, @price_amount, @price_currency)";

		SqlCommand Command = new SqlCommand(InsertStockTransaction, Connection);
		Command.Parameters.AddWithValue("@portfolio", PortfolioId);
		Command.Parameters.AddWithValue("@ticker", Ticker);
		Command.Parameters.AddWithValue("@exchange", Exchange);
		Command.Parameters.AddWithValue("@amount", Amount);
		Command.Parameters.AddWithValue("@amount_adjusted", Amount); //TODO: Should be adjusted in the future
		Command.Parameters.AddWithValue("@amount_owned", Amount); //TODO: Should be calculated in the future
		Command.Parameters.AddWithValue("@timestamp", Timestamp);
		Command.Parameters.AddWithValue("@price_amount", Amount);
		Command.Parameters.AddWithValue("@price_currency", Currency);
		Command.ExecuteNonQuery();

		return this;
	}

	public Portfolio GetPortfolio()
	{
		return new Portfolio(PortfolioId);
	}
}