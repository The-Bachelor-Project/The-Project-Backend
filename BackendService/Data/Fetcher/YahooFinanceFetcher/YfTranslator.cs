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
		{"NYSE", ""},
		{"NASDAQ", ""},
		{"AMS", ".AS"},
{"ATH", ".AT"},
{"ASX", ".AX"},
{"BCBA", ".BA"},
{"BUD", ".BD"},
{"BKK", ".BK"},
{"BOM", ".BO"},
{"NYSE", ".BO"},
{"EBR", ".BR"},
{"CPH", ".CO"},
{"ETR", ".DE"},
{"DUS", ".DU"},
{"FRA", ".F"},
{"FKA", ".F"},
{"HAN", ".HA"},
{"HEL", ".HE"},
{"HKG", ".HK"},
{"HAM", ".HM"},
{"ICE", ".IC"},
{"STO", ".IL"},
{"ISE", ".IR"},
{"IST", ".IS"},
{"JKT", ".JK"},
{"KOSDAQ", ".KQ"},
{"KSC", ".KS"},
{"KUW", ".KW"},
{"LON", ".L"},
{"ELI", ".LS"},
{"MCE", ".MC"},
{"BIT", ".MI"},
{"MUN", ".MU"},
{"BMV", ".MX"},
{"NSE", ".NS"},
{"NZE", ".NZ"},
{"OSL", ".OL"},
{"EPA", ".PA"},
{"PRA", ".PR"},
{"DOH", ".QA"},
{"RSE", ".RG"},
{"SAP", ".S"},
{"SAO", ".SA"},
{"STU", ".SG"},
{"SGX", ".SI"},
{"SGO", ".SN"},
{"TADAWUL", ".SR"},
{"SHA", ".SS"},
{"EBS", ".SW"},
{"SHE", ".SZ"},
{"JPE", ".T"},
{"TLV", ".TA"},
{"TAL", ".TL"},
{"TPE", ".TW"},
{"VIE", ".VI"},
{"LIT", ".VS"},
{"WSE", ".WA"}
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
			throw new StatusCodeException(404, "Could not convert exchange of " + exchange + ":" + ticker + " to Yahoo Finance symbol.");
		}

	}
}