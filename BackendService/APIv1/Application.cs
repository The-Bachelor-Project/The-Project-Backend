namespace API.v1;

public class Application
{
	public static void Setup(Boolean startServer)
	{
		var allowCORS = "_allowCors";
		WebApplicationBuilder builder = WebApplication.CreateBuilder();
		builder.Services.AddCors(options =>
		{
			options.AddPolicy(name: allowCORS,
							policy =>
							{
								policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
							});
		});
		builder.Services.AddControllers();
		builder.Services.AddTransient<Authentication.Middleware>();
		builder.Services.AddTransient<ErrorHandlingMiddlware>();
		WebApplication app = builder.Build();
		app.UseCors(allowCORS);

		app.UseMiddleware<Authentication.Middleware>();
		app.UseMiddleware<ErrorHandlingMiddlware>();
		PostUsers.Setup(app); // This is where the endpoints are setup
		GetUsers.Setup(app);
		GetStockProfiles.Setup(app);
		GetPortfolios.Setup(app);
		PostPortfolios.Setup(app);
		GetStockHistories.Setup(app);
		PutTokens.Setup(app);
		PostTokens.Setup(app);
		GetSearchResults.Setup(app);
		GetStockTransactions.Setup(app);
		PostStockTransactions.Setup(app);
		GetCurrencyHistories.Setup(app);
		GetTransactions.Setup(app);
		GetValueHistory.Setup(app);
		PutUsers.Setup(app);
		PutPortfolios.Setup(app);
		PutStockTransactions.Setup(app);
		DeleteStockTransactions.Setup(app);
		DeletePortfolios.Setup(app);
		DeleteUsers.Setup(app);
		PostCashTransactions.Setup(app);
		PostUserPreferences.Setup(app);
		GetUserPreferences.Setup(app);
		DeleteCashTransactions.Setup(app);
		PutCashTransactions.Setup(app);
		if (startServer) // This is only false for when testing the setup of everything
		{
			app.Run();
		}
	}
}