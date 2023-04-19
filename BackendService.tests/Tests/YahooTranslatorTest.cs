using BackendService;

namespace BackendService.tests;

[TestClass]
public class YahooTranslatorTest
{

	[TestMethod]
	public void TestAllExchangesHasExtension()
	{
		Assert.IsFalse(false, "dwdwd wf missing yahoo symbol extension");
		foreach (KeyValuePair<string, string> entry in Data.YahooFinance.YfTranslator.stockAutocomplete)
		{
			bool result = Data.YahooFinance.YfTranslator.stockSymbolExtension.TryGetValue(entry.Value, out _);
			Assert.IsTrue(result, entry.Value + " is missing yahoo symbol extension");
		}
	}
}