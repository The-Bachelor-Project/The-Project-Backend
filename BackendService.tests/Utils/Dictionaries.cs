namespace BackendService.tests;

public class Dictionaries
{
	public static Dictionary<string, string> stockDictionary = new Dictionary<string, string>()
	{
		{"SAO", "EQIX34"},
		{"BMV", "1299N"},
		{"STU", "014"},
		{"EBR", "ABI"},
		{"MCE", "480S"},
		{"WSE", "06N"},
		{"HEL", "AALLON"},
		{"ATH", "AAAK"},
		{"JKT", "AALI"},
		{"BKK", "A5"},
		{"BUD", "4IG"},
		{"EPA", "AAA"},
		{"NSE", "20MICRONS"},
		{"EBS", "AAM"},
		{"KUW", "UNICAP"},
		{"MUN", "4DN"},
		{"FKA", "1771"},
		{"DUS", "SSS"},
		{"SHE", "000001"},
		{"PRA", "ATS"},
		{"ETR", "02M"},
		{"TLV", "ABRA"},
		{"ICE", "MAREL"},
		{"BOM", "1STCUS"},
		{"HKG", "0001"},
		{"CPH", "AAB"},
		{"ASX", "14D"},
		{"DOH", "ABQK"},
		{"STO", "0A02"},
		{"SGX", "1A1"},
		{"LON", "0A05"},
		{"JPE", "1301"},
		{"NZE", "AFI"},
		{"SHA", "600000"},
		{"SGO", "BAC"},
		{"KSC", "000020"},
		{"TADAWUL", "1010"},
		{"KOSDAQ", "000250"},
		{"LIT", "AMG1L"},
		{"SAP", "1449"},
		{"NASDAQ", "AADI"},
		{"ISE", "A5G"},
		{"RSE", "BAL1R"},
		{"VIE", "1COV"},
		{"FRA", "HO1"},
		{"OSL", "2020"},
		{"NYSE", "A"},
		{"IST", "ACSEL"},
		{"TPE", "1101"},
		{"TAL", "ARC1T"},
		{"BCBA", "AAPL"},
		{"HAM", "VVX"},
		{"BIT", "A2A"},
		{"ELI", "ALTR"}
	};

	public static List<string> currencies = new List<string>
	{
		// Excluding USD as this is tested reguarly
		"ARS",
		"AUD",
		"BRL",
		"CAD",
		"CHF",
		"CLP",
		"CNY",
		"CZK",
		"DKK",
		"EUR",
		"GBP",
		"GBX",
		"HKD",
		"HUF",
		"IDR",
		"ILS",
		"INR",
		"JPY",
		"KRW",
		"KWD",
		"MXN",
		"NOK",
		"NZD",
		"PLN",
		"QAR",
		"SAR",
		"SEK",
		"SGD",
		"THB",
		"TRY",
		"TWD"
	};
}