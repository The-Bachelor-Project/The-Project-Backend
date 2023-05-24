namespace API.v1;

public class GetValueHistoryResponse
{
	public Data.UserAssetsValueHistory valueHistory { get; set; }
	public String? error { get; set; }

	public GetValueHistoryResponse(Data.UserAssetsValueHistory valueHistory)
	{
		this.valueHistory = valueHistory;
	}
}