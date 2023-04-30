using System.Data.SqlClient;

namespace Data.Database;

public class Connection
{
	static SqlConnection? connection = null;
	public SqlConnection Create()
	{
		SqlConnectionStringBuilder builder = buildConnectionString();
		String connectionString = builder.ConnectionString;
		if (connection == null)
		{
			connection = new SqlConnection(connectionString);
			connection.Open();
		}
		return connection;
	}

	private SqlConnectionStringBuilder buildConnectionString()
	{
		SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
		builder.DataSource = "stock-app-db-server.database.windows.net";
		builder.UserID = "bachelor";
		builder.Password = "Gustav.Frederik";
		builder.InitialCatalog = "stock_app_db";
		return builder;
	}
}

