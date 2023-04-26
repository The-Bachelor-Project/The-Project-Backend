namespace BackendService.tests;

using Data;

[TestClass]
public class YFStockProfileFetcherTest
{
	[TestMethod]
	public async Task YFNasdaqProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("tsla", "nasdaq");
		Assert.IsFalse(result == null, "Result is null");
		Assert.IsTrue(result.name == "Tesla, Inc.", "Result name is not correct, should be Tesla, Inc. but is " + result.name);
	}
	[TestMethod]
	public async Task YFNyseProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("bgs", "nyse");
		Assert.IsFalse(result == null, "Result is null");
		Assert.IsTrue(result.name == "B&G Foods, Inc. B&G Foods, Inc.", "Result name is not correct, should be B&G Foods, Inc. B&G Foods, Inc. but is " + result.name);
	}

	[TestMethod]
	public async Task YFCphProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("chemm", "cph");
		Assert.IsFalse(result == null, "Result is null");
		Assert.IsTrue(result.name == "ChemoMetec A/S", "Result name is not correct, should be ChemoMetec A/S but is " + result.name);
	}

	[TestMethod]
	public async Task YFStoProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("sand", "sto");
		Assert.IsFalse(result == null, "Result is null");
		Assert.IsTrue(result.name == "Sandvik AB", "Result name is not correct, should be Sandvik AB but is " + result.name);
	}

	[TestMethod]
	public async Task YFTseProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("BCE", "TSE");
		Assert.IsFalse(result == null, "Result is null");
		Assert.IsTrue(result.name == "BCE INC.", "Result name is not correct, should be BCE INC. but is " + result.name);
	}

	[TestMethod]
	public async Task YFLonProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("BP", "LON");
		Assert.IsFalse(result == null, "Result is null");
		Assert.IsTrue(result.name == "BP PLC $0.25", "Result name is not correct, should be BP PLC $0.25 but is " + result.name);
	}

	[TestMethod]
	public async Task YFHelProfileFetcherTest()
	{
		StockProfile result = await new Data.Fetcher.YahooFinanceFetcher.StockFetcher().GetProfile("ORNBV", "HEL");
		Assert.IsFalse(result == null, "Result is null");
		Assert.IsTrue(result.name == "Orion Corporation B", "Result name is not correct, should be Orion Corporation B but is " + result.name);
	}
}