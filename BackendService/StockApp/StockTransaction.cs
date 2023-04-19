using System.Data.SqlClient;

namespace StockApp;

public class StockTransaction
{
	public String? Id { get; set; }
	public String? PortfolioId { get; set; }
	public String? Ticker { get; set; }
	public String? Exchange { get; set; }
	public Decimal? Amount { get; set; }
	public Decimal? AmountAdjusted { get; set; }
	public Decimal? AmountOwned { get; set; }
	public int? Timestamp { get; set; }
	//public String? Currency { get; set; }
	//public Decimal? Price { get; set; }

	public Money Price { get; set; }



	public async Task<StockTransaction> AddToDb() //FIXME at some point also ORDER by index as well as timestamp to avoid some issues with calculating amount_owned 
	{
		SqlConnection Connection = new Data.Database.Connection().Create();

		try
		{
			await new Data.Fetcher.StockFetcher().GetProfile(Ticker!, Exchange!);
		}
		catch (Exception)
		{
			throw new CouldNotGetStockException();
		}

		String GetAmountOwned = "SELECT TOP 1 amount_owned FROM StockTransactions WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp < @timestamp ORDER BY timestamp DESC";
		SqlCommand Command3 = new SqlCommand(GetAmountOwned, Connection);
		Command3.Parameters.AddWithValue("@portfolio", PortfolioId);
		Command3.Parameters.AddWithValue("@ticker", Ticker);
		Command3.Parameters.AddWithValue("@exchange", Exchange);
		Command3.Parameters.AddWithValue("@timestamp", Timestamp);
		SqlDataReader Reader = Command3.ExecuteReader();



		decimal AmountOwned = Reader.Read() ? Reader.GetDecimal(0) : 0;

		decimal AmountAdjusted = Amount!.Value; //TODO: Should be adjusted in the future
		Reader.Close();


		String InsertStockTransaction = "INSERT INTO StockTransactions(portfolio, ticker, exchange, amount, amount_adjusted, amount_owned, timestamp, price_amount, price_currency) VALUES (@portfolio, @ticker, @exchange, @amount, @amount_adjusted, @amount_owned, @timestamp, @price_amount, @price_currency)";
		SqlCommand Command = new SqlCommand(InsertStockTransaction, Connection);
		Command.Parameters.AddWithValue("@portfolio", PortfolioId);
		Command.Parameters.AddWithValue("@ticker", Ticker);
		Command.Parameters.AddWithValue("@exchange", Exchange);
		Command.Parameters.AddWithValue("@amount", Amount);
		Command.Parameters.AddWithValue("@amount_adjusted", AmountAdjusted);
		Command.Parameters.AddWithValue("@amount_owned", AmountOwned + AmountAdjusted);
		Command.Parameters.AddWithValue("@timestamp", Timestamp);
		Command.Parameters.AddWithValue("@price_amount", Price.amount);
		Command.Parameters.AddWithValue("@price_currency", Price.currency);
		Command.ExecuteNonQuery();



		String UpdateStockTransactions = "UPDATE StockTransactions SET amount_owned = amount_owned + @amount_adjusted WHERE portfolio = @portfolio AND ticker = @ticker AND exchange = @exchange AND timestamp > @timestamp";
		SqlCommand Command2 = new SqlCommand(UpdateStockTransactions, Connection);
		Command2.Parameters.AddWithValue("@portfolio", PortfolioId);
		Command2.Parameters.AddWithValue("@ticker", Ticker);
		Command2.Parameters.AddWithValue("@exchange", Exchange);
		Command2.Parameters.AddWithValue("@amount_adjusted", AmountAdjusted);
		Command2.Parameters.AddWithValue("@timestamp", Timestamp);
		Command2.ExecuteNonQuery();



		return this; //TODO Update Id before returning, maybe by making a function on the database
	}

	public Portfolio GetPortfolio()
	{
		return new Portfolio(PortfolioId!);
	}
}