using System.Data.SqlClient;

namespace BackendService;

class ValidateUserToken
{
    public static bool authenticate(String user_id, String device)
    {
        String getToken = "SELECT token FROM Tokens WHERE user_id = @user_id AND device = @device";
        using (SqlConnection connection = Database.createConnection())
        {
            SqlCommand command = new SqlCommand(getToken, connection);
            command.Parameters.AddWithValue("@user_id", user_id);
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