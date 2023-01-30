using System.Data.SqlClient;

namespace BackendService;

class AddStockTransaction
{
    public static AddStockTransactionResponse endpoint(AddStockTransaction body)
    {
        AddStockTransactionResponse addStockTransactionResponse = new AddStockTransactionResponse();
        using (SqlConnection connection = Database.createConnection())
        {

        }

        return addStockTransactionResponse;
    }
}

class AddStockTransactionResponse
{
    public String response { get; set; }
}

class AddStockTransactionBody
{
    public String str1 { get; set; }
    public String str2 { get; set; }
}