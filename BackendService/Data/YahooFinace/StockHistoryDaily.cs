using BackendService;
using Data.Interfaces;

namespace Data.YahooFinance;

public class StockHistoryDaily : IStockHistoryDaily
{
	public StockHistoryDaily()
	{
	}

	public async Task<StockHistory> usd(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
	{
		int StartTime = Tools.TimeConverter.dateOnlyToUnix(startDate);
		int EndTime = Tools.TimeConverter.dateOnlyToUnix(endDate);
		String TickerExt = YfTranslator.getYfSymbol(ticker, exchange);
		HttpClient Client = new HttpClient();
		String Url = "https://query1.finance.yahoo.com/v7/finance/download/" + TickerExt + "?interval=1d&period1=" + StartTime + "&period2=" + EndTime;

		HttpResponseMessage StockHistoryRes;

		try{
			System.Console.WriteLine("test 1");
			HttpResponseMessage test = new HttpResponseMessage();
			test = await Client.GetAsync(Url);
			System.Console.WriteLine("test 2");
			StockHistoryRes = test;
			System.Console.WriteLine("test 3");
		} catch(NotSupportedException){
			System.Console.WriteLine("test 4");
			return new StockHistory(ticker, exchange, "daily");
			System.Console.WriteLine("test 5");
		}

		if (StockHistoryRes.StatusCode == System.Net.HttpStatusCode.NotFound){
			System.Console.WriteLine("test 6");
			return new StockHistory(ticker, exchange, "daily");
			System.Console.WriteLine("test 7");
		}

		String StockHistoryCsv = await StockHistoryRes.Content.ReadAsStringAsync();
		String[] DataLines = StockHistoryCsv.Replace("\r", "").Split("\n");
		String CurrencySymbol = DatabaseService.Exchange.GetCurrency(exchange);
		bool DoCurrencyConvert = !(CurrencySymbol == "USD" || CurrencySymbol == "usd");
		Dictionary<String, CurrencyHistoryData> Rates = new Dictionary<string, CurrencyHistoryData>();
		//TODO Do this with a function implementing an interface
		if (DoCurrencyConvert)
		{
			await CurrencyRatesUpdater.Update(CurrencySymbol, startDate);
			await CurrencyConverter.GetRatesAsync(startDate, CurrencySymbol);
		}
		//TODO update so the currency converter just returns ones when usd to usd, and then drop DoCurrencyConvert bool
		StockHistory Result = new StockHistory(ticker, exchange, startDate, endDate, "daily");
		List<string> DataList = DataLines.ToList();
		DataList.RemoveAt(0);

		foreach(string Data in DataList){
			String[] DataSplit = Data.Split(",");
			StockHistoryData DataPoint = new StockHistoryData(
				DateOnly.Parse(DataSplit[0]),
				Decimal.Parse(DataSplit[1]),
				Decimal.Parse(DataSplit[2]),
				Decimal.Parse(DataSplit[3]),
				Decimal.Parse(DataSplit[4])
			);
			Result.History = Result.History.Append(DataPoint).ToArray();
		}

		return Result;
	}
}