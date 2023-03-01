using System.Data.SqlClient;

namespace BackendService;


class StockProfile
{
	public String? ticker { get; set; }
	public String? exchange { get; set; }
	public String? name { get; set; }
	public String? industry { get; set; }
	public String? sector { get; set; }
	public String? website { get; set; }
	public String? country { get; set; }
	int trackingDate;


	public static async Task<StockProfileResponse> endpoint(StockProfileBody body)
	{

		try
		{
			return new StockProfileResponse("success", await DatabaseService.StockProfile.Get(body.ticker, body.exchange));
		}
		catch (Exception e)
		{
			System.Console.WriteLine(e);
			throw new CouldNotGetStockException();
		}
	}
}


class StockProfileResponse
{
	public StockProfileResponse(String response, Data.StockProfile stock)
	{
		this.response = response;
		this.stock = stock;
	}
	public StockProfileResponse(String response)
	{
		this.response = response;
	}

	public String response { get; set; }
	public Data.StockProfile? stock { get; set; }
}

class StockProfileBody
{
	public StockProfileBody(String token, String ticker, String exchange)
	{
		this.token = token;
		this.ticker = ticker;
		this.exchange = exchange;
	}
	public String token { get; set; }
	public String ticker { get; set; }
	public String exchange { get; set; }
}