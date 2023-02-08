using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BackendService;
class Search
{
	public static async Task<SearchResponse> endpoint(SearchBody body) //TODO: not done at all
	{
		Regex regex = new Regex("[A-Za-z0-9]*[A-Za-z0-9]", RegexOptions.IgnoreCase);
		MatchCollection matchedAuthors = regex.Matches(body.term);
		String termTrimmed = matchedAuthors[0].Value.ToLower();
		for (int i = 1; i < matchedAuthors.Count; i++)
		{
			termTrimmed += " " + matchedAuthors[i].Value.ToLower();
		}
		SearchResponse searchResponse = new SearchResponse("error");
		if (body.stocks)
		{
			await YfSearch.searchStocksAsync(body.term);
			using (SqlConnection connection = Database.createConnection())
			{
				String query = "SELECT TOP 100 * FROM Stocks WHERE tags LIKE @tags";
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@tags", "%" + body.term + "%");
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					searchResponse.stocks = searchResponse.stocks.Append(new StockSearchResult((String)reader["company_name"], (String)reader["ticker"], (String)reader["exchange"])).ToArray();
				}
				searchResponse.response = "success";
				reader.Close();
			}
		}
		return searchResponse;
	}
}

class SearchResponse
{
	public SearchResponse(string response)
	{
		this.response = response;
		this.stocks = new StockSearchResult[] { };
	}

	public String response { get; set; }
	public StockSearchResult[] stocks { get; set; }
}

class SearchBody
{
	public SearchBody(string term, bool stocks)
	{
		this.term = term;
		this.stocks = stocks;
	}

	public String term { get; set; }
	public bool stocks { get; set; }
}

class StockSearchResult
{
	public StockSearchResult(string name, string ticker, string exchange)
	{
		this.name = name;
		this.ticker = ticker;
		this.exchange = exchange;
	}

	public String name { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
}