namespace API.v1;

class GetCurrencyHistoriesResponse
{
	public GetCurrencyHistoriesResponse(string response, Data.CurrencyHistory history)
	{
		this.response = response;
		this.history = history;
	}

	public String response { get; set; }
	public Data.CurrencyHistory history { get; set; }
}