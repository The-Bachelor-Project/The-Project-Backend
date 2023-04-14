using BackendService;
using Data.Interfaces;

namespace Data.YahooFinance;

public class StockHistoryDaily : IStockHistoryDaily
{
	public StockHistoryDaily()
	{
	}

	public async Task<StockHistory> Usd(String ticker, String exchange, DateOnly startDate, DateOnly endDate)
	{
		System.Console.WriteLine("Fetching stock history for " + ticker + " on " + exchange + " from " + startDate + " to " + endDate);
		int StartTime = Tools.TimeConverter.dateOnlyToUnix(startDate);
		int EndTime = Tools.TimeConverter.dateOnlyToUnix(endDate);
		String TickerExt = YfTranslator.getYfSymbol(ticker, exchange);

		HttpClient Client = new HttpClient();
		String Url = "https://query1.finance.yahoo.com/v7/finance/download/" + TickerExt + "?interval=1d&period1=" + StartTime + "&period2=" + EndTime;
		//System.Console.WriteLine(Url);
		HttpResponseMessage StockHistoryRes = await Client.GetAsync(Url);

		if (StockHistoryRes.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return new StockHistory(ticker, exchange, "daily");
		}

		String StockHistoryCsv = await StockHistoryRes.Content.ReadAsStringAsync();
		String[] DataLines = StockHistoryCsv.Replace("\r", "").Split("\n");
		String CurrencySymbol = DatabaseService.Exchange.GetCurrency(exchange);

		//TODO update so the currency converter just returns ones when usd to usd, and then drop DoCurrencyConvert bool

		StockHistory Result = new StockHistory(ticker, exchange, startDate, endDate, "daily");
		List<string> DataList = DataLines.ToList();
		DataList.RemoveAt(0);

		foreach (string Data in DataList)
		{
			String[] DataSplit = Data.Split(",");
			DateOnly Date = DateOnly.Parse(DataSplit[0]);
			if (Date >= startDate && Date <= endDate)
			{
				StockHistoryData DataPoint = new StockHistoryData(
					DateOnly.Parse(DataSplit[0]),
					Decimal.Parse(DataSplit[1]),
					Decimal.Parse(DataSplit[2]),
					Decimal.Parse(DataSplit[3]),
					Decimal.Parse(DataSplit[4])
				);
				Result.History = Result.History.Append(DataPoint).ToArray();
			}
		}

		return Result;
	}
}