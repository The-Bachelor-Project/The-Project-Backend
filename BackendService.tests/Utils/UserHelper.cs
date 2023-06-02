namespace BackendService.tests;
using StockApp;
using System.Data.SqlClient;
public class UserHelper
{
	public static UserTestObject Create()
	{
		String email = Tools.RandomString.Generate(200) + "@test.com";
		String password = Tools.RandomString.Generate(200);
		String uid = Tools.RandomString.Generate(32);
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		String insertAccountQuery = "INSERT INTO Accounts (user_id, email, password) VALUES (@user_id, @email, @password)";
		SqlCommand command = new SqlCommand(insertAccountQuery, connection);
		command.Parameters.AddWithValue("@user_id", uid);
		command.Parameters.AddWithValue("@email", email);
		command.Parameters.AddWithValue("@password", Tools.Password.Hash(password));
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new StatusCodeException(500, "Failed to create user");
		}
		User user = new User(email, password);
		user.id = uid;
		int familyID = TokenHelper.CreateFamily(uid);
		StockApp.TokenSet tokenSet = TokenHelper.CreateTokens(user, familyID);

		return new UserTestObject(user, tokenSet.accessToken!, tokenSet.refreshToken!, familyID);
	}

	public static void Delete(UserTestObject userTestObject)
	{
		String deleteQuery = "DELETE FROM Accounts WHERE email = @email";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		SqlCommand command = new SqlCommand(deleteQuery, connection);
		command.Parameters.AddWithValue("@email", userTestObject.user!.email);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
		}

		String deleteTokenFamilyQuery = "DELETE FROM TokenFamily WHERE id = @familyID";
		command = new SqlCommand(deleteTokenFamilyQuery, connection);
		command.Parameters.AddWithValue("@familyID", (int)userTestObject.familyID!);
		try
		{
			command.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
		}
	}

	public static UserTestObject GetPreferences(UserTestObject userTestObject)
	{
		String getPreferencesQuery = "SELECT * FROM AccountPreferences WHERE user_id = @user_id";
		SqlConnection connection = Data.Database.Connection.GetSqlConnection();
		Dictionary<String, object> parameters = new Dictionary<string, object>();
		parameters.Add("@user_id", userTestObject.user!.id!);
		Dictionary<String, object>? data = Data.Database.Reader.ReadOne(getPreferencesQuery, parameters);
		if (data is null)
		{
			throw new StatusCodeException(500, "Failed to get preferences");
		}
		userTestObject.setting = (String)data["setting"];
		userTestObject.settingValue = (String)data["value"];
		return userTestObject;
	}
}

public class UserTestObject
{
	public User? user { get; set; }
	public String? accessToken { get; set; }
	public String? refreshToken { get; set; }
	public int? familyID { get; set; }
	public String? setting { get; set; }
	public String? settingValue { get; set; }
	public UserTestObject(User user, String accessToken, String refreshToken, int familyID)
	{
		this.user = user;
		this.accessToken = accessToken;
		this.refreshToken = refreshToken;
		this.familyID = familyID;
	}
	public UserTestObject()
	{

	}
}
