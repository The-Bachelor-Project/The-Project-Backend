namespace Data.Fetcher.YahooFinanceFetcher;
public class YfTranslator
{
	public static Dictionary<String, String> stockAutocomplete = new Dictionary<string, string>(){
		{"FRA","FRA"},
		{"CPH","CPH"},
		{"NYQ","NYSE"},
		{"NAS","NASDAQ"},
		{"NMS","NASDAQ"},
		{"NCM","NASDAQ"},
		{"IOB","STO"},
		{"TOR","TSE"}
	};
	public static Dictionary<string, string> stockSymbolExtension = new Dictionary<string, string>()
	{
		{"FRA", ".F"},
		{"ETR", ".DE"},
		{"CPH", ".CO"},
		{"NYSE", ""},
		{"NASDAQ", ""},
		{"STO", ".ST"},
		{"TSE", ".TO"},
		{"HEL", ".HE"},
		{"LON", ".L"},
		{"BCBA", ".BA"},
		{"VIE", ".VI"},
		{"ASX", ".AX"},
		{"EBR", ".BR"},
		{"BVMF", ".SA"},
		{"SWX", ".SW"},
		{"SHA", ".SS"},
		{"SHE", ".SZ"},
		{"HKG", ".HK"},
		{"IDX", ".JK"},
		{"TLV", ".TA"},
		{"BOM", ".BO"},
		{"NSE", ".NS"},
		{"ICE", ".IC"},
		{"TYO", ".T"},
		{"KORSDAQ", ".KQ"},
		{"KRX", ".KS"},
		{"RSE", ".RG"},
		{"BMV", ".MX"},
		{"AMS", ".AS"},
		{"NZE", ".NZ"},
		{"WSE", ".WA"},
		{"ELI", ".LS"},
		{"SGX", ".SI"},
		{"BKK", ".BK"},
		{"IST", ".IS"},
		{"TPE", ".TW"},
		{"TPO", ".TWO"}
	};

	public static String GetYfSymbol(String ticker, String exchange)
	{
		try
		{
			String? stockExtension;
			stockSymbolExtension.TryGetValue(exchange.ToUpper(), out stockExtension);
			return ticker + stockExtension!.ToLower();
		}
		catch (Exception)
		{
			throw new CouldNotGetStockException("Could not convert exchange of " + exchange + ":" + ticker + " to Yahoo Finance symbol.");
		}

	}
}