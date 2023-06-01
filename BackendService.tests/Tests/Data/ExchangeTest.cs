namespace BackendService.tests;

[TestClass]
public class ExchangeTest
{
	[TestMethod]
	public void ExchangeTest_InvalidExchange()
	{
		StatusCodeException statusCodeException = Assert.ThrowsException<StatusCodeException>(() => Data.Database.Exchange.GetCurrency("invalid"));
		Assert.IsTrue(statusCodeException.StatusCode == 404, "Status code should be 400 but was " + statusCodeException.StatusCode);
	}
}