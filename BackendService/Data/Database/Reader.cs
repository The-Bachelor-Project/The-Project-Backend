using System.Data.SqlClient;

namespace Data.Database;

public class Reader
{
	private static bool blocking = false;

	public static List<Dictionary<String, object>> ReadData(String query, Dictionary<String, object> parameters)
	{
		while (blocking)
		{
			System.Threading.Thread.Sleep(10);
		}
		blocking = true;

		SqlConnection connection = Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(query, connection);

		foreach (KeyValuePair<String, object> parameter in parameters)
		{
			command.Parameters.AddWithValue(parameter.Key, parameter.Value);
		}

		using (SqlDataReader reader = command.ExecuteReader())
		{
			List<Dictionary<String, object>> data = new List<Dictionary<String, object>>();
			while (reader.Read())
			{
				Dictionary<String, object> row = new Dictionary<String, object>();
				for (int i = 0; i < reader.FieldCount; i++)
				{
					row.Add(reader.GetName(i), reader.GetValue(i));
				}
				data.Add(row);
			}
			reader.Close();

			blocking = false;
			return data;
		}
	}

	public static List<Dictionary<String, object>> ReadData(String query)
	{
		return ReadData(query, new Dictionary<String, object>());
	}

	public static Dictionary<String, object>? ReadOne(String query, Dictionary<String, object> parameters)
	{
		List<Dictionary<String, object>> data = ReadData(query, parameters);

		if (data.Count > 0)
		{
			return data[0];
		}
		else
		{
			return null;
		}
	}

	public static Dictionary<String, object>? ReadOne(String query)
	{
		return ReadOne(query, new Dictionary<String, object>());
	}

	//Function to convert a Dictionaries to a string of keys
	public static String DictionaryToString(Dictionary<String, object> dictionary)
	{
		String result = "";
		foreach (KeyValuePair<String, object> entry in dictionary)
		{
			result += entry.Key + ", ";
		}
		return result;
	}
}