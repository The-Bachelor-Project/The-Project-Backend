using System.Data.SqlClient;
class SignUp
{
    // public static String connectionString = "Server=tcp:stock-database-server.database.windows.net,1433;Initial Catalog=stock_app_db;Persist Security Info=False;User ID=bachelor;Password=Gustav.Frederik;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    // public static SqlConnection connection = new SqlConnection(connectionString);
    public SignUpResponse endpoint(SignUpBody body)
    {
        SignUpResponse signUpResponse = new SignUpResponse();
        signUpResponse.response = body.email + " " + body.password;
        using (SqlConnection connection = Database.createConnection())
        {
            String query = "INSERT INTO Account (email, password) VALUES (@email, @password)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", body.email);
            command.Parameters.AddWithValue("@password", body.password);
            command.ExecuteNonQuery();
        }
        
        return signUpResponse;
    }
}

class SignUpResponse
{
    public String response { get; set; }
}

class SignUpBody
{
    public String email { get; set; }
    public String password { get; set; }
}