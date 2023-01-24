class YfTranslator{
	public static Dictionary<String, String> stockAutocomplete = new Dictionary<string, string>(){
		{"FRA","FRA"},
		{"CPH","CPH"},
		{"NYQ","NYSE"},
		{"NAS","NASDAQ"},
		{"NMS","NASDAQ"},
		{"NCM","NASDAQ"}
	};
	public static Dictionary<String, String> stockSymbolExtension = new Dictionary<string, string>(){
		{"FRA",".F"},
		{"CPH",".CO"},
		{"NYSE",""},
		{"NASDAQ",""}
	};
}