using System.Data.SqlClient;
using Data;

namespace BackendService;

class CreatePortfolio
{
	public static CreatePortfolioResponse endpoint(CreatePortfolioBody body)
	{
		System.Console.WriteLine("AddPortfolio endpoint called");
		CreatePortfolioResponse createPortfolioResponse = new CreatePortfolioResponse("error");
		System.Console.WriteLine("AddPortfolio endpoint called");
		DatabaseService.Portfolio.Create(body.portfolio);
		System.Console.WriteLine("AddPortfolio endpoint called");


		return createPortfolioResponse;
	}
}

class CreatePortfolioResponse
{
	public CreatePortfolioResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class CreatePortfolioBody
{
	public CreatePortfolioBody(Portfolio portfolio, string token)
	{
		this.portfolio = portfolio;
		this.token = token;
	}

	public Data.Portfolio portfolio { get; set; }
	public String token { get; set; }
}