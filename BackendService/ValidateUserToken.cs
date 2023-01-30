using System.Data.SqlClient;

namespace BackendService;

class ValidateUserToken
{
    public static bool authenticate(String token, String device)
    {
        // ? How do we handle device??
        String getToken = "SELECT token FROM Tokens WHERE token = @token AND device = @device";
        using (SqlConnection connection = Database.createConnection())
        {
            SqlCommand command = new SqlCommand(getToken, connection);
            command.Parameters.AddWithValue("@token", token);
            command.Parameters.AddWithValue("@device", device);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }

        }

    }
}