namespace API.v1;

class Application
{
	public static void setup()
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
		WebApplication app = builder.Build();
		app.UseCors(allowCORS);

		PostUsers.Setup(app); // This is where the endpoints are setup
		GetUsers.Setup(app);
		GetStockProfiles.Setup(app);
		GetPortfolios.Setup(app);
		PostPortfolios.Setup(app);
		GetStockHistories.Setup(app);
		GetTokens.Setup(app);
		PostTokens.Setup(app);
		//GetStockTransactions.Setup(app);
		//PostStockTransactions.Setup(app);
		//GetSearchResults.Setup(app);



		app.Run();
	}
}