using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace BackendService;

class Program
{
	public static void Main()
	{

		var allowCORS = "_allowCors";
		Application.builder.Services.AddCors(options =>
		{
			options.AddPolicy(name: allowCORS,
							  policy =>
							  {
								  policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
							  });
		});
		Application.builder.Services.AddControllers();
		Application.app = Application.builder.Build();
		Application.app.UseCors(allowCORS);
		Endpoints.setup();
		Application.app.Run();

	}
}



//await DataFetcher.stock("ibm","nyse");

