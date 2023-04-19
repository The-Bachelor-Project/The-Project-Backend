using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Data.Interfaces;

namespace Data.Database;

class StockProfile : IStockProfile
{
	public Task<Data.StockProfile> Get(string ticker, string exchange)
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

		return Task.FromResult(Profile);
	}

	public Task<Data.StockProfile[]> Search(string query)
	{
		Data.StockProfile[] results = new Data.StockProfile[] { };
		Regex regex = new Regex("[A-Za-z0-9]*[A-Za-z0-9]", RegexOptions.IgnoreCase);
		MatchCollection matchedAuthors = regex.Matches(query);
		String termTrimmed = matchedAuthors[0].Value.ToLower();
		for (int i = 1; i < matchedAuthors.Count; i++)
		{
			termTrimmed += " " + matchedAuthors[i].Value.ToLower();
		}

		SqlConnection connection = new Connection().Create();
		String SqlQuery = "SELECT TOP 100 * FROM Stocks WHERE tags LIKE @tags";
		SqlCommand command = new SqlCommand(SqlQuery, connection);
		command.Parameters.AddWithValue("@tags", "%" + query + "%");
		SqlDataReader reader = command.ExecuteReader();
		while (reader.Read())
		{
			results = results.Append(new Data.StockProfile((String)reader["ticker"], (String)reader["exchange"], (String)reader["company_name"])).ToArray();
		}
		reader.Close();
		return Task.FromResult(results);
	}
}