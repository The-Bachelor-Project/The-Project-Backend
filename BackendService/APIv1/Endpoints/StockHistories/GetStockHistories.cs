using Microsoft.AspNetCore.Mvc;

namespace API.v1;
public class GetStockHistories
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-histories", async ([FromQuery] string ticker, string exchange, string startDate, string endDate, string interval, string currency) =>
		{
			return Results.Ok(await Endpoint(ticker, exchange, startDate, endDate, interval, currency));
		});
	}

	public static async Task<GetStockHistoriesResponse> Endpoint(string ticker, string exchange, string startDate, string endDate, string interval, string currency)
	{
		Data.StockHistory result = await (new Data.Fetcher.StockFetcher()).GetHistory(ticker, exchange, DateOnly.Parse(startDate), DateOnly.Parse(endDate), "daily", currency);
		return new GetStockHistoriesResponse("success", result);
	}
}
