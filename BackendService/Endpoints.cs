using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace BackendService;

class Endpoints
{
	public static void setup()
	{
		Application.app.MapPost(API_ENDPOINTS.SIGN_UP_ENDPOINT, (SignUpBody body) =>
		{
			return Results.Ok(SignUp.endpoint(body));
		});

		Application.app.MapPost(API_ENDPOINTS.SIGN_IN_ENDPOINT, (SignInBody body) =>
		{
			return Results.Ok(SignIn.endpoint(body));
		});

		Application.app.MapPost(API_ENDPOINTS.STOCK_INFO_ENDPOINT, async (StockProfileBody body) =>
		{
			return Results.Ok(await StockProfile.endpoint(body));
		});

		Application.app.MapPost(API_ENDPOINTS.STOCK_HISTORY_ENDPOINT, async (StockHistoryBody body) =>
		{
			return Results.Ok(await StockHistory.endpoint(body));
		});

		Application.app.MapPost(API_ENDPOINTS.SEARCH_ENDPOINT, async (SearchBody body) =>
		{
			return Results.Ok(await Search.endpoint(body));
		});

		Application.app.MapPost(API_ENDPOINTS.ADD_PORTFOLIO, (AddPortfolioBody body) =>
		{
			return Results.Ok(AddPortfolio.endpoint(body));
		});

		Application.app.MapPost(API_ENDPOINTS.ADD_STOCK_TRANSACTION_ENDPOINT, (AddStockTransactionBody body) =>
		{
			return Results.Ok(AddStockTransaction.endpoint(body));
		});

		Application.app.MapPost(API_ENDPOINTS.STOCK_TAG_GENERATION, () =>
		{
			DatabaseService.StockTagGenerator.updateAllStocks();
		});
	}
}