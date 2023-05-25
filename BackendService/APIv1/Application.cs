namespace API.v1;

class Application
{
	public static void Setup()
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
		GetTokens.Setup(app);
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

		app.Run();
	}
}