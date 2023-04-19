using Microsoft.AspNetCore.Mvc;

namespace API.v1;
class GetStockHistories
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-histories", async ([FromQuery] string ticker, string exchange, string startDate, string endDate, string interval, string accessToken) =>
		{
			return Results.Ok(await Endpoint(ticker, exchange, startDate, endDate, interval, accessToken));
		});
	}

	public static async Task<GetStockHistoriesResponse> Endpoint(string ticker, string exchange, string startDate, string endDate, string interval, string accessToken)
	{
		Data.StockHistory Result = await (new Data.Fetcher.StockHistoryDaily()).Usd(ticker, exchange, DateOnly.Parse(startDate), DateOnly.Parse(endDate));
		return new GetStockHistoriesResponse("success", Result);
	}
}
