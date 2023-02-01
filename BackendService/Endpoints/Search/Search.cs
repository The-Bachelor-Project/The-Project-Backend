using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BackendService;
class Search
{
    public static async Task<SearchResponse> endpoint(SearchBody body) //TODO: not done at all
    {
        Regex regex = new Regex("[A-Za-z0-9]+[A-Za-z0-9]+", RegexOptions.IgnoreCase);
        MatchCollection matchedAuthors = regex.Matches(body.term);
        String termTrimmed = matchedAuthors[0].Value.ToLower();
        for (int i = 1; i < matchedAuthors.Count; i++)
        {
            termTrimmed += " " + matchedAuthors[i].Value.ToLower();
        }
        String response = "";
        if (body.stocks)
        {
            await YfSearch.searchStocksAsync(body.term);
            using (SqlConnection connection = Database.createConnection())
            {
                System.Console.WriteLine(body.term);
                String query = "SELECT TOP 100 * FROM Stocks WHERE tags LIKE @tags";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tags", "%" + body.term + "%");
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    response += reader["exchange"].ToString() + ":" + reader["ticker"].ToString() + ", ";
                }
                reader.Close();
            }
        }

        SearchResponse searchResponse = new SearchResponse(response);
        return searchResponse;
    }
}

class SearchResponse
{
    public SearchResponse(string response)
    {
        this.response = response;
    }

    public String response { get; set; }
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