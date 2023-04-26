namespace StockApp.EndpointHandler
{
	public class ValueHistory : EndpointHandler
	{
		public ValueHistory(String accessToken) : base(accessToken)
		{

		}

		public async Task<Data.UserAssetsValueHistory> Get(String currency, DateOnly startDate, DateOnly endDate)
		{
			user.UpdatePortfolios();
			return await user.GetValueHistory(currency, startDate, endDate);
			throw new NotImplementedException();
		}
	}
}