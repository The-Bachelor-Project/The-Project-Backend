using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DatabaseService;

class Search
{
	static public async Task<Data.StockProfile[]> Stocks(String term)
	{
		Data.StockProfile[] results = new Data.StockProfile[] { };
		Regex regex = new Regex("[A-Za-z0-9]*[A-Za-z0-9]", RegexOptions.IgnoreCase);
		MatchCollection matchedAuthors = regex.Matches(term);
		String termTrimmed = matchedAuthors[0].Value.ToLower();
		for (int i = 1; i < matchedAuthors.Count; i++)
		{
			termTrimmed += " " + matchedAuthors[i].Value.ToLower();
		}

		await BackendService.YfSearch.searchStocksAsync(term);
		SqlConnection connection = Database.createConnection();
		String query = "SELECT TOP 100 * FROM Stocks WHERE tags LIKE @tags";
		SqlCommand command = new SqlCommand(query, connection);
		command.Parameters.AddWithValue("@tags", "%" + term + "%");
		SqlDataReader reader = command.ExecuteReader();
		while (reader.Read())
		{
			results = results.Append(new Data.StockProfile((String)reader["company_name"], (String)reader["ticker"], (String)reader["exchange"])).ToArray();
		}
		reader.Close();
		return results;
	}
}