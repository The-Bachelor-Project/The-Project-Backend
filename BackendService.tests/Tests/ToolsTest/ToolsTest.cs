namespace BackendService.tests;


[TestClass]
public class ToolsTest
{
	[TestMethod]
	public void ToolsTest_RandomStringTest()
	{
		String randomString = Tools.RandomString.Generate(255);
		Assert.IsTrue(randomString.Length == 255, "randomString should have length 10 but has length " + randomString.Length);
		Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(randomString, "^[a-zA-Z0-9]*$"), "randomString should only contain letters and numbers but contains \"" + randomString + "\"");

	}
	[TestMethod]
	public void ToolsTest_PasswordHashTest()
	{
		String password = Tools.RandomString.Generate(20);
		String passwordHash = Tools.Password.Hash(password);
		Assert.IsTrue(passwordHash.Length == 64, "passwordHash should have length 64 but has length " + passwordHash.Length);
		Assert.IsTrue(password != passwordHash, "password should not be the same as hashed password");
	}

	[TestMethod]
	public void ToolsTest_DateOnlyUnixTest()
	{
		DateOnly date = new DateOnly(2021, 1, 1);
		int unixConverted = Tools.TimeConverter.DateOnlyToUnix(date);
		DateOnly dateConverted = Tools.TimeConverter.UnixTimeStampToDateOnly(unixConverted);
		Assert.IsTrue(dateConverted == date, "date should be the same as date converted from unix but was " + dateConverted.ToString() + " instead of " + date.ToString());
	}

	[TestMethod]
	public void ToolsTest_ValidEmailTest()
	{
		String email = Tools.RandomString.Generate(10) + "@test.com";
		Boolean isValid = Tools.ValidEmail.Check(email);
		Assert.IsTrue(isValid, email + " should be true for valid but was " + isValid);
	}

	[TestMethod]
	public void ToolsTest_InvalidEmailTest()
	{
		String email = Tools.RandomString.Generate(10) + "test.com";
		Boolean isValid = Tools.ValidEmail.Check(email);
		Assert.IsFalse(isValid, email + " should be false for valid but was " + isValid);
	}
}