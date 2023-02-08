using System.Data.SqlClient;

namespace BackendService;

class EndpointTemplate
{
	public static EndpointTemplateResponse endpoint(EndpointTemplateBody body)
	{
		EndpointTemplateResponse endpointTemplateResponse = new EndpointTemplateResponse("error");
		using (SqlConnection connection = Database.createConnection())
		{
			String query = "INSERT INTO Table (val1, val2) VALUES (@val1, @val2)";
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@val1", body.str1);
			command.Parameters.AddWithValue("@val2", body.str2);
			try
			{
				command.ExecuteNonQuery();
				endpointTemplateResponse.response = "success";
			}
			catch (Exception e)
			{
				endpointTemplateResponse.response = "error";
			}
		}

		return endpointTemplateResponse;
	}
}

class EndpointTemplateResponse
{
	public EndpointTemplateResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class EndpointTemplateBody
{
	public EndpointTemplateBody(string str1, string str2)
	{
		this.str1 = str1;
		this.str2 = str2;
	}

	public String str1 { get; set; }
	public String str2 { get; set; }
}