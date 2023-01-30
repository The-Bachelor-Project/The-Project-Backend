using System.Data.SqlClient;

namespace BackendService;

class SignIn
{
    public static SignInResponse endpoint(SignInBody body)
    {
        SignInResponse signInResponse = new SignInResponse();
        using (SqlConnection connection = Database.createConnection())
        {
            String query = "SELECT * FROM Accounts WHERE email = @email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", body.email);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                String dbPassword = reader["password"].ToString();
                reader.Close();
                String token = handleTokenGeneration(connection, body);
                if (token != "")
                {
                    signInResponse.response = "success";
                    signInResponse.token = token;
                }
                else
                {
                    signInResponse.response = "Error generating tokens";
                }
            }
            else
            {

                signInResponse.response = "error";
            }

        }

        return signInResponse;
    }

    private static String handleTokenGeneration(SqlConnection connection, SignInBody body)
    {
        String checkIfDeviceExists = "SELECT device FROM Tokens WHERE user_id = (SELECT user_id FROM Accounts WHERE email = @email) AND device = @device";
        SqlCommand command = new SqlCommand(checkIfDeviceExists, connection);
        command.Parameters.AddWithValue("@email", body.email);
        command.Parameters.AddWithValue("@device", body.device);
        SqlDataReader reader = command.ExecuteReader();
        String uid = RandomStringGenerator.Generate(32);
        if (reader.Read())
        {
            reader.Close();
            String updateTokenOnDevice = "UPDATE Tokens SET token = @uid WHERE user_id = (SELECT user_id FROM Accounts WHERE email = @email) AND device = @device";
            command = new SqlCommand(updateTokenOnDevice, connection);
            command.Parameters.AddWithValue("@uid", uid);
            command.Parameters.AddWithValue("@email", body.email);
            command.Parameters.AddWithValue("@device", body.device);
            try
            {
                command.ExecuteNonQuery();
                return uid;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return "";
            }
        }
        else
        {
            reader.Close();
            String insertTokenOnDevice = "INSERT INTO Tokens(user_id, device, token) VALUES ((SELECT user_id FROM Accounts WHERE email = @email), @device, @uid)";
            command = new SqlCommand(insertTokenOnDevice, connection);
            command.Parameters.AddWithValue("@email", body.email);
            command.Parameters.AddWithValue("@device", body.device);
            command.Parameters.AddWithValue("@uid", uid);
            try
            {
                command.ExecuteNonQuery();
                return uid;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return "";
            }
        }
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
    public String device { get; set; }
    public String password { get; set; }
}