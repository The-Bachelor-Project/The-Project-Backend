using System;
using System.Data.SqlClient;
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
		{"JPX","JPE"},
		{"LSE","LON"},
		{"BSE","BOM"},
		{"SHZ","SHE"},
		{"HKG","HKG"},
		{"SHH","SHA"},
		{"NSI","NSE"},
		{"ASX","ASX"},
		{"SET","BKK"},
		{"KOE","KOSDAQ"},
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
	static Dictionary<string, int> errors = new Dictionary<string, int>();

	public static void Main()
	{
		SqlConnection connection = Create();
		// Specify the folder path
		string folderPath = "../profiles_json";

		// Get all JSON files in the folder
		string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

		System.Console.WriteLine("Found " + jsonFiles.Length + " JSON files");

		// String array to store extracted key values
		//string[] extractedValues = new string[jsonFiles.Length];

		List<Dictionary<string, dynamic>> profiles = new List<Dictionary<string, dynamic>>();
		Dictionary<string, int> missingExchanges = new Dictionary<string, int>();
		Dictionary<string, dynamic> yahooCode = new Dictionary<string, dynamic>();

		Dictionary<string, Dictionary<string, string>> stocks = new Dictionary<string, Dictionary<string, string>>();

		int totalCounter = 0;
		int missingCounter = 0;
		int iDunnoCounter = 0;
		int duplicateCounter = 0;

		int profilesAdded = 0;
		int profilesFailed = 0;

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
					catch
					{
						AddError("displayName");
					}

					profile.Add("longName", "");
					try
					{
						profile["longName"] = jsonObject["quote"]["result"][0]["longName"].ToString();
					}
					catch
					{
						AddError("longName");
					}

					profile.Add("shortName", "");
					try
					{
						profile["shortName"] = jsonObject["quote"]["result"][0]["shortName"].ToString();
					}
					catch
					{
						AddError("shortName");
					}


					profile.Add("financialCurrency", "");
					try
					{
						profile["financialCurrency"] = jsonObject["quote"]["result"][0]["financialCurrency"].ToString();
					}
					catch
					{
						AddError("financialCurrency");
					}

					profile.Add("sharesOutstanding", "");
					try
					{
						profile["sharesOutstanding"] = jsonObject["quote"]["result"][0]["sharesOutstanding"].ToString();
					}
					catch
					{
						AddError("sharesOutstanding");
					}


					profile.Add("ticker", "");
					try
					{
						profile["ticker"] = jsonObject["quote"]["result"][0]["symbol"].ToString().Split('.')[0];
					}
					catch
					{
						AddError("ticker");
					}

					if (jsonObject["quote"]["result"][0]["symbol"].ToString().Split('.').Length > 2)
					{
						throw new Exception("Too many dots in symbol");
						AddError("Too many dots in symbol");
					}


					profile.Add("exchange", "");
					try
					{
						profile["exchange"] = exchanges[jsonObject["quote"]["result"][0]["exchange"].ToString()];
						System.Console.WriteLine(jsonFiles[i]);
						yahooCode.TryAdd(exchanges[jsonObject["quote"]["result"][0]["exchange"].ToString()], jsonFiles[i].Split("_")[2].Split(".")[0]);
					}
					catch
					{
						AddError("exchange");
					}

					profile.Add("address", "");
					try
					{
						profile["address"] = jsonObject["summary"]["result"][0]["assetProfile"]["address1"].ToString();
					}
					catch
					{
						AddError("address 1");
					}

					try
					{
						profile["address"] += ", " + jsonObject["summary"]["result"][0]["assetProfile"]["address2"].ToString();
					}
					catch
					{
						AddError("address 2");
					}

					profile.Add("city", "");
					try
					{
						profile["city"] = jsonObject["summary"]["result"][0]["assetProfile"]["city"].ToString();
					}
					catch
					{
						AddError("city");
					}

					profile.Add("state", "");
					try
					{
						profile["state"] = jsonObject["summary"]["result"][0]["assetProfile"]["state"].ToString();
					}
					catch
					{
						AddError("state");
					}

					profile.Add("zip", "");
					try
					{
						profile["zip"] = jsonObject["summary"]["result"][0]["assetProfile"]["zip"].ToString();
					}
					catch
					{
						AddError("zip");
					}

					profile.Add("country", "");
					try
					{
						profile["country"] = jsonObject["summary"]["result"][0]["assetProfile"]["country"].ToString();
					}
					catch
					{
						AddError("country");
					}




					profile.Add("website", "");
					try
					{
						profile["website"] = jsonObject["summary"]["result"][0]["assetProfile"]["website"].ToString();
					}
					catch
					{
						AddError("website");
					}

					profile.Add("industry", "");
					try
					{
						profile["industry"] = jsonObject["summary"]["result"][0]["assetProfile"]["industry"].ToString();
					}
					catch
					{
						AddError("industry");
					}

					profile.Add("sector", "");
					try
					{
						profile["sector"] = jsonObject["summary"]["result"][0]["assetProfile"]["sector"].ToString();
					}
					catch
					{
						AddError("sector");
					}


					// Extract the key value you want from the JSON data
					//string extractedValue = jsonObject["key"].ToString(); // Replace "key" with your desired key

					// Save the extracted value to the array
					//extractedValues[i] = extractedValue;

					if (!stocks.ContainsKey(profile["exchange"]))
					{
						stocks.Add(profile["exchange"], new Dictionary<string, string>());
					}

					if (!stocks[profile["exchange"]].ContainsKey(profile["ticker"]))
					{
						stocks[profile["exchange"]].Add(profile["ticker"], profile["displayName"]);
					}
					else
					{
						System.Console.WriteLine("Duplicate: " + profile["exchange"] + " : " + profile["ticker"] + " : " + profile["displayName"] + " : " + jsonFiles[i]);
						duplicateCounter++;
						throw new Exception("Duplicate");
					}

					profiles.Add(profile);

					String tags = GenerateTags(profile);
					String query = "INSERT INTO Stocks (ticker, exchange, company_name, short_name, long_name, address, city, state, zip, financial_currency, shares_outstanding, industry, sector, website, country, tags) VALUES (@ticker, @exchange, @name, @short_name, @long_name, @address, @city, @state, @zip, @financial_currency, @shares_outstanding, @industry, @sector, @website, @country, @tags)";
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@ticker", profile["ticker"]);
					command.Parameters.AddWithValue("@exchange", profile["exchange"]);
					command.Parameters.AddWithValue("@name", profile["displayName"]);
					command.Parameters.AddWithValue("@short_name", profile["shortName"]);
					command.Parameters.AddWithValue("@long_name", profile["longName"]);
					command.Parameters.AddWithValue("@address", profile["address"]);
					command.Parameters.AddWithValue("@city", profile["city"]);
					command.Parameters.AddWithValue("@state", profile["state"]);
					command.Parameters.AddWithValue("@zip", profile["zip"]);
					command.Parameters.AddWithValue("@financial_currency", profile["financialCurrency"]);
					command.Parameters.AddWithValue("@shares_outstanding", profile["sharesOutstanding"]);
					command.Parameters.AddWithValue("@industry", profile["industry"]);
					command.Parameters.AddWithValue("@sector", profile["sector"]);
					command.Parameters.AddWithValue("@website", profile["website"]);
					command.Parameters.AddWithValue("@country", profile["country"]);
					command.Parameters.AddWithValue("@tags", tags);
					try
					{
						//command.ExecuteNonQuery();
						System.Console.WriteLine("Inserted: " + profile["exchange"] + " : " + profile["ticker"] + " : " + profile["displayName"] + " : " + jsonFiles[i]);

						profilesAdded++;
					}
					catch (Exception e)
					{
						AddError("SQL");
						System.Console.WriteLine(e.Message);
						profilesFailed++;
					}

					System.Threading.Thread.Sleep(2);
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
				System.Console.WriteLine(e);
				iDunnoCounter++;
			}



			// Convert the string array to JSON
			//string extractedValuesJson = JsonConvert.SerializeObject(extractedValues, Formatting.Indented);

			// Save the JSON array to a file
			//string outputPath = @"C:\Your\Output\File.json";
			//File.WriteAllText(outputPath, extractedValuesJson);

			//Console.WriteLine("Extraction completed. Results saved to " + outputPath);
		}
		string extractedValuesJson = JsonConvert.SerializeObject(profiles, Formatting.None);
		//string extractedValuesJson = JsonConvert.SerializeObject(new Dictionary<string, dynamic>() { { "profiles", profiles } }, Formatting.None);
		//String jsonString = GetJson(new Dictionary<string, dynamic>() { { "profiles", profiles } });
		string outputPath = "result.json";
		File.WriteAllText(outputPath, extractedValuesJson);





		//missingExchanges = missingExchanges.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
		//printDictInt(missingExchanges);

		System.Console.WriteLine("\nTotal: " + totalCounter);
		System.Console.WriteLine("Missing: " + missingCounter);
		System.Console.WriteLine("I dunno: " + (iDunnoCounter - duplicateCounter));
		System.Console.WriteLine("Duplicates: " + duplicateCounter);
		System.Console.WriteLine("Profiles added: " + profilesAdded);
		System.Console.WriteLine("Profiles failed: " + profilesFailed);

		printDictInt(errors);
		System.Console.WriteLine("\n");

		yahooCode = yahooCode.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
		printDict(yahooCode);
	}

	//function that prints all key value pairs in a dictionary
	public static void printDict(Dictionary<string, dynamic> dict)
	{
		foreach (KeyValuePair<string, dynamic> kvp in dict)
		{
			Console.WriteLine("{\"" + kvp.Key + "\", \"." + kvp.Value + "\"},");
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
	public static SqlConnection Create()
	{
		SqlConnectionStringBuilder builder = buildConnectionString();
		String connectionString = builder.ConnectionString;
		SqlConnection connection = new SqlConnection(connectionString);
		connection.Open();
		return connection;
	}
	private static SqlConnectionStringBuilder buildConnectionString()
	{
		SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
		builder.DataSource = "stock-app-db-server.database.windows.net";
		builder.UserID = "bachelor";
		builder.Password = "Gustav.Frederik";
		builder.InitialCatalog = "stock_app_db";
		return builder;
	}
	public static String GenerateTags(Dictionary<string, dynamic> stockProfile)
	{
		String tags = "";
		tags += stockProfile["exchange"] + " " + stockProfile["ticker"] + ",";
		tags += stockProfile["ticker"] + " " + stockProfile["exchange"] + ",";
		tags += stockProfile["displayName"] + ",";
		tags += stockProfile["shortName"] + ",";
		tags += stockProfile["longName"] + ",";
		return tags.ToLower();
	}
	public static void AddError(String error)
	{
		if (!errors.ContainsKey(error))
		{
			errors.Add(error, 1);
		}
		else
		{
			errors[error]++;
		}
	}
}
