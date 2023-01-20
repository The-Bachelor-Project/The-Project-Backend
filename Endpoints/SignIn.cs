using System.Data.SqlClient;
class SignIn
{
    public SignInResponse endpoint(SignInBody body)
    {
        SignInResponse signInResponse = new SignInResponse();
        using (SqlConnection connection = Database.createConnection())
        {
            String query = "SELECT * FROM Accounts WHERE email = @email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", body.email);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read()){
                String dbPassword = reader["password"].ToString();
                signInResponse.response = "👍";
                signInResponse.token = body.email;
            }
            else{

                signInResponse.response = "👎";
            }

        }
        
        return signInResponse;
    }
}

class SignInResponse
{
    public String response { get; set; }
    public String token { get; set; }
}

class SignInBody
{
    public String email { get; set; }
    public String password { get; set; }
}