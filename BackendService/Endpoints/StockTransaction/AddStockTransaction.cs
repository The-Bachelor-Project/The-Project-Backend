using System.Data.SqlClient;
using Data;

namespace BackendService;

class AddStockTransaction
{
	public static async Task<AddStockTransactionResponse> endpoint(AddStockTransactionBody body)
	{
		AddStockTransactionResponse addStockTransactionResponse = new AddStockTransactionResponse("errorgf");
		try
		{
			await DatabaseService.StockTransaction.Add(body.transaction);
			addStockTransactionResponse.response = "success";
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e);
			addStockTransactionResponse.response = "erroruu";
		}
		return addStockTransactionResponse;
	}
}

class AddStockTransactionResponse
{
	public AddStockTransactionResponse(string response)
	{
		this.response = response;
	}

	public String response { get; set; }
}

class AddStockTransactionBody
{
	public AddStockTransactionBody(StockTransaction transaction, string token)
	{
		this.transaction = transaction;
		this.token = token;
	}

	public Data.StockTransaction transaction { get; set; }
	public String token { get; set; }
}