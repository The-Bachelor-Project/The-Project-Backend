using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
	public static Dictionary<String, String> exchanges = new Dictionary<string, string>(){
		{"FRA","FRA"},
		{"HAM","HAM"},
		{"AMS","AMS"},
		{"OSL","OSL"},
		{"STU","STU"},
		{"CPH","CPH"},
		{"NYQ","NYSE"},
		{"NAS","NASDAQ"},
		{"NMS","NASDAQ"},
		{"NCM","NASDAQ"},
		{"IOB","STO"},
		{"TOR","TSE"},
		{"STO","STO"},
		{"PNK","OTCMKTS"},
		{"JPX","TSE"},
		{"LSE","LON"},
		{"BSE","BOM"},
		{"SHZ","SHE"},
		{"HKG","HKG"},
		{"SHH","SHA"},
		{"NSI","NSE"},
		{"ASX","ASX"},
		{"SET","BKK"},
		{"KOE","KOSDAQ"},
		{"CXE","CXE"},
		{"SAO","SAO"},
		{"TAI","TPE"},
		{"TWO","TPE"},
		{"KSC","KSC"},
		{"VIE","VIE"},
		{"JKT","JKT"},
		{"GER","ETR"},
		{"PAR","EPA"},
		{"MEX","BMV"},
		{"TLV","TLV"},
		{"SES","SGX"},
		{"IST","IST"},
		{"MUN","MUN"},
		{"WSE","WSE"},
		{"NGM","NGM"},
		{"MIL","BIT"},
		{"SAU","TADAWUL"},
		{"EBS","EBS"},
		{"BUE","BCBA"},
		{"HEL","HEL"},
		{"MCE","MCE"},
		{"ASE","ASE"},
		{"ATH","ATH"},
		{"KUW","KUW"},
		{"SGO","SGO"},
		{"NZE","NZE"},
		{"BRU","EBR"},
		{"DUS","DUS"},
		{"DOH","DOH"},
		{"LIS","ELI"},
		{"PRA","PRA"},
		{"ICE","ICE"},
		{"ISE","ISE"},
		{"LIT","LIT"},
		{"BUD","BUD"},
		{"FKA","FKA"},
		{"TAL","TAL"},
		{"HAN","HAN"},
		{"SAP","SAP"},
		{"RIS","RSE"},
	};

	public static void Main()
	{
		// Specify the folder path
		string folderPath = "../profiles_json";

		// Get all JSON files in the folder
		string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

		System.Console.WriteLine("Found " + jsonFiles.Length + " JSON files");

		// String array to store extracted key values
		//string[] extractedValues = new string[jsonFiles.Length];

		List<Dictionary<string, dynamic>> profiles = new List<Dictionary<string, dynamic>>();
		Dictionary<string, int> missingExchanges = new Dictionary<string, int>();

		int totalCounter = 0;
		int missingCounter = 0;
		int iDunnoCounter = 0;

		// Process each JSON file
		for (int i = 0; i < jsonFiles.Length; i++)
		{
			System.Console.WriteLine("Processing file " + (i + 1) + " of " + jsonFiles.Length + " ( " + jsonFiles[i] + " )");
			Dictionary<string, dynamic> profile = new Dictionary<string, dynamic>();
			string filePath = jsonFiles[i];
			// Read the JSON file
			string jsonData = File.ReadAllText(filePath);
			// Parse the JSON data
			JObject jsonObject = JObject.Parse(jsonData);
			//System.Console.WriteLine(jsonObject["quote"]["result"][0].ToString());
			try
			{

				if (exchanges.ContainsKey(jsonObject["quote"]["result"][0]["exchange"].ToString()))
				{
					totalCounter++;
					profile.Add("displayName", "");
					try
					{
						profile["displayName"] = jsonObject["quote"]["result"][0]["displayName"].ToString();
					}
					catch { }

					profile.Add("longName", "");
					try
					{
						profile["longName"] = jsonObject["quote"]["result"][0]["longName"].ToString();
					}
					catch { }

					profile.Add("shortName", "");
					try
					{
						profile["shortName"] = jsonObject["quote"]["result"][0]["shortName"].ToString();
					}
					catch { }


					profile.Add("financialCurrency", "");
					try
					{
						profile["financialCurrency"] = jsonObject["quote"]["result"][0]["financialCurrency"].ToString();
					}
					catch { }



					profile.Add("ticker", "");
					try
					{
						profile["ticker"] = jsonObject["quote"]["result"][0]["symbol"].ToString().Split('.')[0];
					}
					catch { }

					if (jsonObject["quote"]["result"][0]["symbol"].ToString().Split('.').Length > 2)
						throw new Exception("Too many dots in symbol");

					profile.Add("exchange", "");
					try
					{
						profile["exchange"] = exchanges[jsonObject["quote"]["result"][0]["exchange"].ToString()];
					}
					catch { }

					profile.Add("address", "");
					try
					{
						profile["address"] = jsonObject["summary"]["result"][0]["assetProfile"]["address1"].ToString();
					}
					catch { }

					try
					{
						profile["address"] += ", " + jsonObject["summary"]["result"][0]["assetProfile"]["address2"].ToString();
					}
					catch { }

					profile.Add("city", "");
					try
					{
						profile["city"] = jsonObject["summary"]["result"][0]["assetProfile"]["city"].ToString();
					}
					catch { }

					profile.Add("state", "");
					try
					{
						profile["state"] = jsonObject["summary"]["result"][0]["assetProfile"]["state"].ToString();
					}
					catch { }

					profile.Add("zip", "");
					try
					{
						profile["zip"] = jsonObject["summary"]["result"][0]["assetProfile"]["zip"].ToString();
					}
					catch { }

					profile.Add("country", "");
					try
					{
						profile["country"] = jsonObject["summary"]["result"][0]["assetProfile"]["country"].ToString();
					}
					catch { }




					profile.Add("website", "");
					try
					{
						profile["website"] = jsonObject["summary"]["result"][0]["assetProfile"]["website"].ToString();
					}
					catch { }

					profile.Add("industry", "");
					try
					{
						profile["industry"] = jsonObject["summary"]["result"][0]["assetProfile"]["industry"].ToString();
					}
					catch { }

					profile.Add("sector", "");
					try
					{
						profile["sector"] = jsonObject["summary"]["result"][0]["assetProfile"]["sector"].ToString();
					}
					catch { }


					// Extract the key value you want from the JSON data
					//string extractedValue = jsonObject["key"].ToString(); // Replace "key" with your desired key

					// Save the extracted value to the array
					//extractedValues[i] = extractedValue;

					profiles.Add(profile);
				}
				else
				{
					missingCounter++;
					if (missingExchanges.ContainsKey(jsonObject["quote"]["result"][0]["exchange"].ToString()))
					{
						missingExchanges[jsonObject["quote"]["result"][0]["exchange"].ToString()]++;
					}
					else
					{
						missingExchanges.Add(jsonObject["quote"]["result"][0]["exchange"].ToString(), 1);
					}
					//System.Console.WriteLine("Processing file " + (i + 1) + " of " + jsonFiles.Length + " ( " + jsonFiles[i] + " )");
					//System.Console.WriteLine(jsonObject["quote"]["result"][0]["exchange"].ToString() + " : " + jsonObject["quote"]["result"][0]["fullExchangeName"].ToString() + " : " + jsonObject["quote"]["result"][0]["symbol"].ToString());
				}
			}
			catch (Exception e)
			{
				iDunnoCounter++;
			}

			// Convert the string array to JSON
			//string extractedValuesJson = JsonConvert.SerializeObject(extractedValues, Formatting.Indented);

			// Save the JSON array to a file
			//string outputPath = @"C:\Your\Output\File.json";
			//File.WriteAllText(outputPath, extractedValuesJson);

			//Console.WriteLine("Extraction completed. Results saved to " + outputPath);
		}
		foreach (Dictionary<string, dynamic> profile in profiles)
		{
			printDict(profile);
		}

		missingExchanges = missingExchanges.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);


		printDictInt(missingExchanges);
		System.Console.WriteLine("\nTotal: " + totalCounter);
		System.Console.WriteLine("Missing: " + missingCounter);
		System.Console.WriteLine("I dunno: " + iDunnoCounter);
	}

	//function that prints all key value pairs in a dictionary
	public static void printDict(Dictionary<string, dynamic> dict)
	{
		foreach (KeyValuePair<string, dynamic> kvp in dict)
		{
			Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
		}
		System.Console.WriteLine("\n");
	}

	public static void printDictInt(Dictionary<string, int> dict)
	{
		foreach (KeyValuePair<string, int> kvp in dict)
		{
			Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
		}
		System.Console.WriteLine("\n");
	}
}
