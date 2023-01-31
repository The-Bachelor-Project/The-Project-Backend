using System.Data.SqlClient;

namespace BackendService;

class SignUp
{
	public static SignUpResponse endpoint(SignUpBody body)
	{
		SignUpResponse signUpResponse = new SignUpResponse("error", "i dunno");
		using (SqlConnection connection = Database.createConnection())
		{
			String getEmailQuery = "SELECT email FROM Accounts WHERE email = @email";
			SqlCommand command = new SqlCommand(getEmailQuery, connection);
			command.Parameters.AddWithValue("@email", body.email);
			SqlDataReader reader = command.ExecuteReader();
			if (!reader.Read())
			{
				reader.Close();
				String UID = RandomStringGenerator.Generate(32);
				String query = "INSERT INTO Accounts (user_id, email, password) VALUES (@user_id, @email, @password)";
				command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@user_id", UID);
				command.Parameters.AddWithValue("@email", body.email);
				command.Parameters.AddWithValue("@password", body.password);
				try
				{
					command.ExecuteNonQuery();
					signUpResponse.response = "success";
					signUpResponse.uid = UID;
				}
				catch (Exception e)
				{
					System.Console.WriteLine(e);
					signUpResponse.response = "error";
				}
			}
			else
			{
				signUpResponse.response = "Email already in use";
			}


		}

		return signUpResponse;
	}
}

class SignUpResponse
{
	public SignUpResponse(string response, string uid)
	{
		this.response = response;
		this.uid = uid;
	}

	public String response { get; set; }
	public String uid { get; set; }
}

class SignUpBody
{
	public SignUpBody(string email, string password)
	{
		this.email = email;
		this.password = password;
	}

	public String email { get; set; }
	public String password { get; set; }
}