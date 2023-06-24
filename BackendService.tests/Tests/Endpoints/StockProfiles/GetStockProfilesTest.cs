namespace BackendService.tests;

[TestClass]
public class GetStockPorfilesTest
{

	[TestMethod]
	public async Task GetStockPorfilesTest_InvalidTickerTest()
	{
		string ticker = "invalid";
		string exchange = "NASDAQ";

		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockProfiles.Endpoint(ticker, exchange));
		Assert.IsTrue(exception.StatusCode == 500, "Status code should be 500 but was " + exception.StatusCode);
	}

	[TestMethod]
	public async Task GetStockPorfilesTest_InvalidExchangeTest()
	{
		string ticker = "AAPL";
		string exchange = "invalid";

		StatusCodeException exception = await Assert.ThrowsExceptionAsync<StatusCodeException>(() => GetStockProfiles.Endpoint(ticker, exchange));
		Assert.IsTrue(exception.StatusCode == 404, "Status code should be 404 but was " + exception.StatusCode);
	}

	// Tests for all exchanges

	[TestMethod]
	public async Task GetStockProfilesTest_OTCMKTS()
	{
		string ticker = "AABVF";
		string exchange = "OTCMKTS";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_NASDAQ()
	{
		string ticker = "AAL";
		string exchange = "NASDAQ";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_KOSDAQ()
	{
		string ticker = "001000";
		string exchange = "KOSDAQ";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_TADAWUL()
	{
		string ticker = "1020";
		string exchange = "TADAWUL";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_BCBA()
	{
		string ticker = "ABEV";
		string exchange = "BCBA";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_NYSE()
	{
		string ticker = "BE";
		string exchange = "NYSE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_FRA()
	{
		string ticker = "2QPA";
		string exchange = "FRA";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_HAM()
	{
		string ticker = "HBM";
		string exchange = "HAM";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);

	}

	[TestMethod]
	public async Task GetStockProfilesTest_AMS()
	{
		string ticker = "BHND";
		string exchange = "AMS";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);

	}

	[TestMethod]
	public async Task GetStockProfilesTest_OSL()
	{
		string ticker = "AKVA";
		string exchange = "OSL";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);

	}

	[TestMethod]
	public async Task GetStockProfilesTest_STU()
	{
		string ticker = "EPL";
		string exchange = "STU";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);

	}

	[TestMethod]
	public async Task GetStockProfilesTest_CPH()
	{
		string ticker = "SAS-DKK";
		string exchange = "CPH";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);

	}

	[TestMethod]
	public async Task GetStockProfilesTest_STO()
	{
		string ticker = "0MHT";
		string exchange = "STO";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);

	}

	[TestMethod]
	public async Task GetStockProfilesTest_LON()
	{
		string ticker = "FWD";
		string exchange = "LON";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_BOM()
	{
		string ticker = "KGPETRO";
		string exchange = "BOM";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_SHE()
	{
		string ticker = "002233";
		string exchange = "SHE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_HKG()
	{
		string ticker = "1809";
		string exchange = "HKG";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_SHA()
	{
		string ticker = "601665";
		string exchange = "SHA";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_NSE()
	{
		string ticker = "MADRASFERT";
		string exchange = "NSE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_ASX()
	{
		string ticker = "IMU";
		string exchange = "ASX";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_BKK()
	{
		string ticker = "SKR";
		string exchange = "BKK";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_SAO()
	{
		string ticker = "EUCA4";
		string exchange = "SAO";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_TPE()
	{
		string ticker = "3416";
		string exchange = "TPE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_KSC()
	{
		string ticker = "014440";
		string exchange = "KSC";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_VIE()
	{
		string ticker = "PSM";
		string exchange = "VIE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_JKT()
	{
		string ticker = "NIRO";
		string exchange = "JKT";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_ETR()
	{
		string ticker = "HHHA";
		string exchange = "ETR";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_EPA()
	{
		string ticker = "KER";
		string exchange = "EPA";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_TLV()
	{
		string ticker = "OPAL";
		string exchange = "TLV";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_SGX()
	{
		string ticker = "BTE";
		string exchange = "SGX";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_IST()
	{
		string ticker = "MIPAZ";
		string exchange = "IST";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_MUN()
	{
		string ticker = "CUP0";
		string exchange = "MUN";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_WSE()
	{
		string ticker = "EUC";
		string exchange = "WSE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_NGM()
	{
		string ticker = "INBX";
		string exchange = "NGM";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_BIT()
	{
		string ticker = "IOT";
		string exchange = "BIT";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_EBS()
	{
		string ticker = "TIBN";
		string exchange = "EBS";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_HEL()
	{
		string ticker = "YIT";
		string exchange = "HEL";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_MCE()
	{
		string ticker = "SEC";
		string exchange = "MCE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_ASE()
	{
		string ticker = "SVM";
		string exchange = "ASE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_ATH()
	{
		string ticker = "KAMP";
		string exchange = "ATH";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_KUW()
	{
		string ticker = "TAMINV";
		string exchange = "KUW";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_SGO()
	{
		string ticker = "LIPIGAS";
		string exchange = "SGO";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_NZE()
	{
		string ticker = "VHP";
		string exchange = "NZE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_EBR()
	{
		string ticker = "OPTI";
		string exchange = "EBR";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_DUS()
	{
		string ticker = "VRL";
		string exchange = "DUS";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_DOH()
	{
		string ticker = "QFBQ";
		string exchange = "DOH";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_ELI()
	{
		string ticker = "MLRZE";
		string exchange = "ELI";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_PRA()
	{
		string ticker = "OTP";
		string exchange = "PRA";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_ICE()
	{
		string ticker = "HAGA";
		string exchange = "ICE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_ISE()
	{
		string ticker = "SK3";
		string exchange = "ISE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_LIT()
	{
		string ticker = "VBL1L";
		string exchange = "LIT";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_BUD()
	{
		string ticker = "AMIXA";
		string exchange = "BUD";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_FKA()
	{
		string ticker = "8560";
		string exchange = "FKA";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_TAL()
	{
		string ticker = "TVE1T";
		string exchange = "TAL";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_HAN()
	{
		string ticker = "HAK";
		string exchange = "HAN";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_SAP()
	{
		string ticker = "9027";
		string exchange = "SAP";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}

	[TestMethod]
	public async Task GetStockProfilesTest_RSE()
	{
		string ticker = "VIRSI";
		string exchange = "RSE";

		GetStockProfilesResponse response = await GetStockProfiles.Endpoint(ticker, exchange);

		Assert.IsTrue(response.response == "success", "Response should be success but was " + response.response);
		Assert.IsTrue(response.stock!.ticker == ticker, "Ticker should be " + ticker + " but was " + response.stock!.ticker);
		Assert.IsTrue(response.stock!.exchange == exchange, "Exchange should be " + exchange + " but was " + response.stock!.exchange);
	}
}