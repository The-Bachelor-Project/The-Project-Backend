using System.Data.SqlClient;
class SignUp
{
    public static SignUpResponse endpoint(SignUpBody body)
    {
        SignUpResponse signUpResponse = new SignUpResponse();
        using (SqlConnection connection = Database.createConnection())
        {
            String UID = RandomStringGenerator.Generate(32, "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890");

            String query = "INSERT INTO Accounts (uid, email, password) VALUES (@uid, @email, @password)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@uid", UID);
            command.Parameters.AddWithValue("@email", body.email);
            command.Parameters.AddWithValue("@password", body.password);
            try{
                command.ExecuteNonQuery();
                signUpResponse.response = "success";
                signUpResponse.uid = UID;
            } catch(Exception e){
                //TODO: do a check if email already exist and return specific error in response
                signUpResponse.response = "error";
            }
        }
        
        return signUpResponse;
    }
}

class SignUpResponse
{
    public String response { get; set; }
    public String uid {get;set;}
}

class SignUpBody
{
    public String email { get; set; }
    public String password { get; set; }
}