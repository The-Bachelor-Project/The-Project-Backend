using Newtonsoft.Json.Linq;

class YfSearch
{
	public static async Task searchStocksAsync(String term){
		System.Console.WriteLine("Test line");
		HttpClient client = new HttpClient();
		HttpResponseMessage autoCompleteRes = await client.GetAsync("https://query1.finance.yahoo.com/v6/finance/autocomplete?region=US&lang=en&query=" + term);
		String autoCompleteResJson = await autoCompleteRes.Content.ReadAsStringAsync();
		dynamic autoComplete = JObject.Parse(autoCompleteResJson);

		JArray results = autoComplete.ResultSet.Result;

		foreach(dynamic res in results){
			if(res.type == "S" || res.type == "s"){
				string exchange = "";
				if(YfTranslator.stockAutocomplete.TryGetValue(""+res.exch, out exchange)){
					String ticker = (""+res.symbol).Split(".")[0];
					await StockInfo.getStock(ticker,exchange);
				}
			}
		}
	}
}

class YfSearchStockResult{
	StockInfo stockInfo;
}

