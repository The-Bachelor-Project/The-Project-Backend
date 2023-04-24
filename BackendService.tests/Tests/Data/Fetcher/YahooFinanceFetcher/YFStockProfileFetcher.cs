namespace BackendService.tests;

using Data;

[TestClass]
public class YFStockProfileFetcherTest
{
	[TestMethod]
	public async Task YFNasdaqProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("tsla", "nasdaq");
		Assert.IsFalse(result == null);
		Assert.IsTrue(result.name == "Tesla, Inc.");
	}
	[TestMethod]
	public async Task YFNyseProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("bgs", "nyse");
		Assert.IsFalse(result == null);
		Assert.IsTrue(result.name == "B&G Foods, Inc. B&G Foods, Inc.");
	}

	[TestMethod]
	public async Task YFCphProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("chemm", "cph");
		Assert.IsFalse(result == null);
		Assert.IsTrue(result.name == "ChemoMetec A/S");
	}

	[TestMethod]
	public async Task YFStoProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("sand", "sto");
		Assert.IsFalse(result == null);
		Assert.IsTrue(result.name == "Sandvik AB");
	}

	[TestMethod]
	public async Task YFTseProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("BCE", "TSE");
		Assert.IsFalse(result == null);
		Assert.IsTrue(result.name == "BCE INC.");
	}

	[TestMethod]
	public async Task YFLonProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("BP", "LON");
		Assert.IsFalse(result == null);
		Assert.IsTrue(result.name == "BP PLC $0.25");
	}

	[TestMethod]
	public async Task YFHelProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("ORNBV", "HEL");
		Assert.IsFalse(result == null);
		Assert.IsTrue(result.name == "Orion Corporation B");
	}
}