using System.Data.SqlClient;
class SignUp
{
    public static SignUpResponse endpoint(SignUpBody body)
    {
        SignUpResponse signUpResponse = new SignUpResponse();
        using (SqlConnection connection = Database.createConnection())
        {
            String query = "INSERT INTO Accounts (email, password) VALUES (@email, @password)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", body.email);
            command.Parameters.AddWithValue("@password", body.password);
            try{
                command.ExecuteNonQuery();
                signUpResponse.response = "👍";
            } catch(Exception e){
                //TODO: do a check if email already exist and return specific error in response
                signUpResponse.response = "👎";
            }
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