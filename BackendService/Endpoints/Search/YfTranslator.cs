namespace BackendService;
public class YfTranslator
{
	public static Dictionary<String, String> stockAutocomplete = new Dictionary<string, string>(){
		{"FRA","FRA"},
		{"CPH","CPH"},
		{"NYQ","NYSE"},
		{"NAS","NASDAQ"},
		{"NMS","NASDAQ"},
		{"NCM","NASDAQ"},
		{"IOB","STO"}
	};
	public static Dictionary<String, String> stockSymbolExtension = new Dictionary<string, string>(){
		{"FRA",".F"},
		{"CPH",".CO"},
		{"NYSE",""},
		{"NASDAQ",""},
		{"STO",".ST"}
	};

	public static String getYfSymbol(String ticker, String exchange)
	{
		String? stockExtension;
		stockSymbolExtension.TryGetValue(exchange, out stockExtension);
		return ticker + stockExtension;
	}
}