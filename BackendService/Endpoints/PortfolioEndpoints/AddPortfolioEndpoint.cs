using System.Data.SqlClient;
using Data;

namespace BackendService;

class AddPortfolio
{
	public static AddPortfolioResponse endpoint(AddPortfolioBody body)
	{
		AddPortfolioResponse addPortfolioResponse = new AddPortfolioResponse("error");
		DatabaseService.Portfolio.Add(body.portfolio);


		return addPortfolioResponse;
	}
}

class AddPortfolioResponse
{
	public AddPortfolioResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class AddPortfolioBody
{
	public AddPortfolioBody(Portfolio portfolio, string token)
	{
		this.portfolio = portfolio;
		this.token = token;
	}

	public Data.Portfolio portfolio { get; set; }
	public String token { get; set; }
}