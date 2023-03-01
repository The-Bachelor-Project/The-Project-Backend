using System.Data.SqlClient;

namespace DatabaseService;

class Database
{
	private static SqlConnectionStringBuilder builder = buildConnectionString();
	private static String connectionString = builder.ConnectionString;
	public static SqlConnection createConnection()
	{
		SqlConnection connection = new SqlConnection(connectionString);
		connection.Open();
		return connection;
	}

	private static SqlConnectionStringBuilder buildConnectionString()
	{
		SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
		builder.DataSource = "stock-app-db-server.database.windows.net";
		builder.UserID = "bachelor";
		builder.Password = "Gustav.Frederik";
		builder.InitialCatalog = "stock_app_db";
		return builder;
	}
}

