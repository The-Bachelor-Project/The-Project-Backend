namespace StockApp.EndpointHandler
{
	public class ValueHistory : EndpointHandler
	{
		public ValueHistory(String accessToken) : base(accessToken)
		{

		}

		public async Task<Data.UserAssetsValueHistory> Get(String currency, DateOnly startDate, DateOnly endDate)
		{
			if (startDate > endDate)
			{
				throw new StatusCodeException(400, "Start date must be before end date");
			}
			user.UpdatePortfolios();
			return await user.GetValueHistory(currency, startDate, endDate);
		}
	}
}