using System.Data.SqlClient;
using Data.Interfaces;

namespace Data.Database;

class StockProfile : IStockProfile
{
	public async Task<Data.StockProfile> Get(string ticker, string exchange)
	{
		Data.StockProfile Profile = new Data.StockProfile();
		Profile.Ticker = ticker;
		Profile.Exchange = exchange;


		SqlConnection Connection = (new Connection()).Create();
		String Query = "SELECT * FROM Stocks WHERE ticker = @ticker AND exchange = @exchange";
		SqlCommand Command = new SqlCommand(Query, Connection);
		Command.Parameters.AddWithValue("@ticker", ticker);
		Command.Parameters.AddWithValue("@exchange", exchange);
		SqlDataReader Reader = Command.ExecuteReader();

		if (Reader.Read())
		{
			Profile.Name = Reader["company_name"].ToString();
			Profile.Industry = Reader["industry"].ToString();
			Profile.Sector = Reader["sector"].ToString();
			Profile.Website = Reader["website"].ToString();
			Profile.Country = Reader["country"].ToString();
		}
		else
		{
			throw new CouldNotGetStockException();
		}

		return Profile;
	}
}