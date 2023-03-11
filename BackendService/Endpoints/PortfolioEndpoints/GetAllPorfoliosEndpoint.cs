using System.Data.SqlClient;
using Data;

namespace BackendService;

class GetAllPortfolios
{
	public static GetAllPortfoliosResponse endpoint(GetAllPortfoliosBody body)
	{
		GetAllPortfoliosResponse getAllPortfoliosResponse = new GetAllPortfoliosResponse(DatabaseService.Portfolio.GetAll(body.owner));
		return getAllPortfoliosResponse;
	}
}

class GetAllPortfoliosResponse
{
	public GetAllPortfoliosResponse(Data.Portfolio[] portfolios)
	{
		this.portfolios = portfolios;
	}

	public Data.Portfolio[] portfolios { get; set; }
}

class GetAllPortfoliosBody
{
	public GetAllPortfoliosBody(string owner)
	{
		this.owner = owner;
	}

	public string owner { get; set; }
}