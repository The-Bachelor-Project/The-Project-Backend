using System.Data.SqlClient;

namespace Data.Database;

public class Connection
{
	private static SqlConnection? connection;

	public static SqlConnection GetSqlConnection()
	{
		if (connection is null)
		{
			Connection.connection = Connection.Create();
		}
		return connection;
	}
	public static SqlConnection Create()
	{
		SqlConnectionStringBuilder builder = buildConnectionString();
		String connectionString = builder.ConnectionString;
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

