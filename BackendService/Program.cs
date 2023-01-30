using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace BackendService;

class Program
{
	public static void Main()
	{
		Endpoints.setup();

		Application.app.Run();
	}
}



//await DataFetcher.stock("ibm","nyse");

