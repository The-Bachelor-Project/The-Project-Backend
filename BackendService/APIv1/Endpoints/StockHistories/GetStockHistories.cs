using Microsoft.AspNetCore.Mvc;

class GetStockHistories
{
	public static void Setup(WebApplication app)
	{
		app.MapGet("/v1/stock-histories", ([FromQuery] string ticker, string exchange, string startDate, string endDate, string interval, string accessToken) =>
		{
			return Results.Ok(Endpoint(ticker, exchange, startDate, endDate, interval, accessToken));
		});
	}

	public static GetStockHistoriesResponse Endpoint(string ticker, string exchange, string startDate, string endDate, string interval, string accessToken)
	{
		return new GetStockHistoriesResponse("success", new Data.StockHistory("1","2","3","4","5"));
	}
}
