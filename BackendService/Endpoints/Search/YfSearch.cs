using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace BackendService;
class YfSearch
{
	public static async Task searchStocksAsync(String term)
	{
		System.Console.WriteLine("Test line");
		HttpClient client = new HttpClient();
		HttpResponseMessage autoCompleteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/autocomplete?region=US&lang=en&query=" + term);
		String autoCompleteResJson = await autoCompleteRes.Content.ReadAsStringAsync();
		dynamic autoComplete = JObject.Parse(autoCompleteResJson);

		JArray results = autoComplete.ResultSet.Result;

		foreach (dynamic res in results)
		{
			if (res.type == "S" || res.type == "s")
			{
				string exchange = "";
				if (YfTranslator.stockAutocomplete.TryGetValue("" + res.exch, out exchange))
				{
					String ticker = ("" + res.symbol).Split(".")[0];
					await StockInfo.getStock(ticker, exchange);
				}
				else
				{
					using (SqlConnection connection = Database.createConnection()) //TODO: This is just for development. Remove before production.
					{
						String query = "INSERT INTO MissingExchanges (exchange, disp, stock) VALUES (@exchange, @disp, @stock)";
						SqlCommand command = new SqlCommand(query, connection);
						command.Parameters.AddWithValue("@exchange", "" + res.exch);
						command.Parameters.AddWithValue("@disp", "" + res.exchDisp);
						command.Parameters.AddWithValue("@stock", "" + res.symbol);
						command.ExecuteNonQuery();
					}
				}
			}
		}
	}
}